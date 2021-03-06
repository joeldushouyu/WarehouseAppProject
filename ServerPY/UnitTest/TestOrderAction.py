import unittest


from Enumerables.ActionType import ActionType
from OrderAction import  OrderAction
from Enumerables.ActionType import ActionType



class OrderActionTestCase(unittest.TestCase):


    def test_to_dict(self):
        orderACtion = OrderAction(id=10 , fromOrderID=2031,action=ActionType(1).name, quantity=10, itemBarcode=22332, locationId=2)
        self.assertEqual(orderACtion.to_dict(), {"id": 10, "fromOrderID":2031, "action": "Pick", "quantity":10, "itemBarcode":22332, "locationId":2},"Error in to_dict()")


    def test_from_dict(self):
        data = {"id": 1, "fromOrderID":20, "action": "Supply", "quantity":10,"itemBarcode":22332, "locationId":2}
        orderAction = OrderAction()
        orderAction.from_dict(data)

        self.assertEqual(orderAction.id,1,"Id not the same")
        self.assertEqual(orderAction.fromOrderID, 20, "OrderId not the same")
        self.assertEqual(orderAction.action,ActionType.Supply, "Order action not the same")
        self.assertEqual(orderAction.quantity, 10, "quantity not the same")
        self.assertEqual(orderAction.itemBarcode,22332,"different item barcode")
        self.assertEqual(orderAction.locationId, 2, "differentLocationId")
if __name__ == '__main__':
    unittest.main()