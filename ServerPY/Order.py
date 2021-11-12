import  datetime
from Enumerables.BoxSize import BoxSize
from OrderAction import  OrderAction
from Database import db
class Order(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    barcodeId = db.Column(db.Integer, nullable=True)
    boxSize = db.Column(db.String(10), nullable=True)
    orderDate = db.Column(db.String(100), nullable=True)
    errorOccurred = db.Column(db.Boolean(False), nullable=False)
    locked = db.Column(db.Boolean(False), nullable=False)
    message =  db.Column(db.String(100), nullable=True)
    orderActionsId = db.Column(db.String(10000), nullable=True)
        # a string contains all the orderActionId, Ex: "1212,2323,3232"

    orderActions = []

    def to_dict(self):
        message = {}
        for i in range(1,len(self.orderActions)+1):
            message["orderAction"+ str(i)] = self.orderActions[i-1].to_dict()
        # have the rest of the sloot to be null




        return {"id":self.id, "barcodeID":self.barcodeId, "boxSize":self.boxSize.name, "orderDate":str(self.orderDate), "errorOccurred":self.errorOccured,
                "locked":self.locked, "message":self.message,
                "orderActions": message
                }

    # exception should be catch when called,
    def from_dict(self,data):
        self.id = data["id"]
        self.barcodeId = data["barcodeID"]
        self.boxSize = BoxSize[data["boxSize"]]
        self.orderDate = datetime.date.fromisoformat(data["orderDate"])
        self.errorOccured = data["errorOccurred"]
        self.locked = data["locked"]
        self.message = data["message"]

        lenth = len(data["orderActions"]) # how many orderActions
        for i in range(1,lenth+1):
            ord = OrderAction()
            ord.from_dict(data["orderActions"]["orderAction" + str(i)])
            self.orderActions.append(ord)


