import Order
from Enumerables import ActionType


class OrderAction():
    def __init__(self, id: int, fromOrderID: int, action: ActionType, quantity: int):
        self.id = id
        self.fromOrderID = fromOrderID  # how the databseId of the order it came from
        self.action = action
        self.quantity = quantity

    def to_dict(self):
        return {"id": self.id, "fromOrderID": self.fromOrderID, "action": self.action.name, "quantity": self.quantity}
