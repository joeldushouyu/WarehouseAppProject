import unittest


from Enumerables.ActionType import ActionType
from Enumerables.ActionType import ActionType
import datetime
from Enumerables.BoxSize import BoxSize
from Order import Order


class OrderAction():
    def __init__(self, id=0, fromOrderID=0, action=ActionType.Supply, quantity=0):
        self.id = id
        self.fromOrderID = fromOrderID  # how the databseId of the order it came from
        self.action = action
        self.quantity = quantity



    def to_dict(self):
        return {"id": self.id, "fromOrderID": self.fromOrderID, "action": self.action.name, "quantity": self.quantity}

    # exception should be handle by its caller
    # should also check if the data has data or not
    def from_dict(self, data:dict):
      self.id = data["id"]
      self.fromOrderID = data["fromOrderID"]
      self.action = ActionType[data["action"]]
      self.quantity = data["quantity"]




class OrderActionTestCase(unittest.TestCase):

    maxDiff = None
    def test_to_dict(self):
        t = datetime.date(2021,10,1)
        ord = Order(1,2323,BoxSize.S , t, "test order")


        orderAction1 = OrderAction()
        orderAction1.from_dict({"id": 1, "fromOrderID":1, "action": "Supply", "quantity":10})

        ord.orderActions.append(orderAction1)
        dictInfo = ord.to_dict()
        self.assertEqual(

            {
                "id":1,
                "barcodeID":2323,
                "boxSize":"S",
                "orderDate":"2021-10-01",
                "errorOccurred":False,
                "locked":False,
                "message":"test order",
                "orderActions":{
                    "orderAction1":{"id": 1, "fromOrderID":1, "action": "Supply", "quantity":10}
                }
            }
            , ord.to_dict(),"errror in dict"
        )
    def test_from_dict(self):

        ord = Order()
        ord.from_dict(
            {
                "id": 1,
                "barcodeID": 2323,
                "boxSize": "S",
                "orderDate": "2021-10-01",
                "errorOccurred": False,
                "locked": True,
                "message": "test",
                "orderActions": {
                    "orderAction1": {"id": 1, "fromOrderID": 1, "action": "Supply", "quantity": 10},
                    "orderAction2": {"id": 4, "fromOrderID": 1, "action": "Supply", "quantity": 10}
                }
            }
        )

        self.assertTrue(ord.id==1,"error in id")
        self.assertTrue(ord.barcodeId == 2323, "barcode Id")
        self.assertTrue(ord.boxSize == BoxSize.S, "boxsize")
        self.assertTrue(ord.orderDate == datetime.date(2021,10,1), "datetime")
        self.assertTrue(ord.errorOccured == False, "errorOccurred")
        self.assertTrue(ord.locked == True, "locked")
        self.assertTrue(ord.message == "test", "message")
        self.assertTrue(len(ord.orderActions) == 2)

if __name__ == '__main__':
    unittest.main()