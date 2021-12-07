import  datetime

import flask

from Enumerables.BoxSize import BoxSize
from OrderAction import  OrderAction
from Database import db,orm
class Order(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    barcodeId = db.Column(db.Integer, nullable=True)
    boxSize = db.Column(db.String(10), nullable=True)
    orderDate = db.Column(db.String(100), nullable=True)
    errorOccurred = db.Column(db.Boolean(False), nullable=False)
    locked = db.Column(db.Boolean(False), nullable=False)
    message =  db.Column(db.String(100), nullable=True)



    @orm.reconstructor
    def __init__(self, **kwargs):
        super(Order, self).__init__(**kwargs)
        self.orderActions = []

    def load_orderActions(self):
        ordActs = OrderAction.query.filter_by(fromOrderID=self.id).all()
        ordActs = sorted(ordActs, key=self.sortOrderActionsPredicate)  #ensure that returns a sorted list of orderactions
        for ord in ordActs:
            if(ord.status == False):  # only give orderActions that is not complete yet
                self.orderActions.append(ord)

    def sortOrderActionsPredicate(self, ord):
        number = ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T",
                  "U",
                  "V", "W", "X", "Y", "Z"]
        # sort the orderActions base on locationID
        return ord.locationId

    def to_dict_jsonify(self):
        return flask.jsonify([ordAct.to_dict() for ordAct in self.orderActions])
    def to_dict(self):
        """message = {}
        for i in range(1,len(self.orderActions)+1):
            message["orderAction"+ str(i)] = self.orderActions[i-1].to_dict()"""
        # have the rest of the sloot to be null




        return {"id":self.id, "barcodeID":self.barcodeId, "boxSize":self.boxSize, "orderDate":str(self.orderDate), "errorOccurred":self.errorOccurred,
                "locked":self.locked, "message":self.message
                }

    # exception should be catch when called,
    def from_dict(self,data):
        self.id = data["id"]
        self.barcodeId = data["barcodeID"]
        self.boxSize = BoxSize[data["boxSize"]]
        self.orderDate = datetime.date.fromisoformat(data["orderDate"])
        self.errorOccurred = data["errorOccurred"]
        self.locked = data["locked"]
        self.message = data["message"]

        lenth = len(data["orderActions"]) # how many orderActions
        for i in range(1,lenth+1):
            ord = OrderAction()
            ord.from_dict(data["orderActions"]["orderAction" + str(i)])
            self.orderActions.append(ord)


