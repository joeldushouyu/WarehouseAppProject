
from Enumerables.ActionType import ActionType
from ServerModel import  db

class OrderAction(db.Model):

    id = db.Column(db.Integer, primary_key=True)
    fromOrderID = db.Column(db.Integer, nullable=False) # how the databseId of the order it came from
    action = db.Column(db.String(100), nullable=True)
    quantity =db.Column(db.Integer, nullable=False)
    itemBarcode = db.Column(db.Integer, nullable=False)



    def to_dict(self):
        return {"id": self.id, "fromOrderID": self.fromOrderID, "action": self.action.name, "quantity": self.quantity
                , "itemBarcode":self.itemBarcode}

    # exception should be handle by its caller
    # should also check if the data has data or not
    def from_dict(self, data:dict):
      self.id = data["id"]
      self.fromOrderID = data["fromOrderID"]
      self.action = ActionType[data["action"]]
      self.quantity = data["quantity"]


