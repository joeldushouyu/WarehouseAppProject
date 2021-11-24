
from Enumerables.ActionType import ActionType
from Database import  db
from Location import Location

class OrderAction(db.Model):

    id = db.Column(db.Integer, primary_key=True)
    fromOrderID = db.Column(db.Integer, nullable=False) # how the databseId of the order it came from
    action = db.Column(db.String(100), nullable=True)
    quantity =db.Column(db.Integer, nullable=False)
    itemBarcode = db.Column(db.Integer, nullable=False)
    locationId = db.Column(db.Integer, nullable=False)
    status = db.Column(db.Boolean(False), nullable=False)



    def to_dict(self):
        return {"id": self.id, "fromOrderID": self.fromOrderID, "action": self.action, "quantity": self.quantity
                , "itemBarcode":self.itemBarcode, "locationId":self.locationId, "status":self.status}

    # exception should be handle by its caller
    # should also check if the data has data or not
    def from_dict(self, data:dict):
      self.id = data["id"]
      self.fromOrderID = data["fromOrderID"]
      self.action = ActionType[data["action"]]
      self.quantity = data["quantity"]
      self.itemBarcode = data["itemBarcode"]
      self.locationId = data["locationId"]




