import unittest


from Enumerables.ActionType import ActionType
from Enumerables.ActionType import ActionType
import datetime
from Enumerables.BoxSize import BoxSize
from Order import Order
from OrderAction import OrderAction






class OrderActionTestCase(unittest.TestCase):

    maxDiff = None
    def test_to_dict(self):
        t = datetime.date(2021,10,1)
        ord = Order(id=1,barcodeId=2323,boxSize=BoxSize.S , orderDate=t, errorOccurred=False, locked=False, message="test order")


        orderAction1 = OrderAction(id=10 , fromOrderID=2031,action=ActionType(1).name, quantity=10, itemBarcode=22332, locationId=2)

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
                    "orderAction1":{"id": 10, "fromOrderID":2031, "action": "Pick", "quantity":10, "itemBarcode":22332, "locationId":2}

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
                    "orderAction1":{"id": 5, "fromOrderID":2031, "action": "Pick", "quantity":10, "itemBarcode":22332, "locationId":2},
                    "orderAction2":{"id": 10, "fromOrderID":2031, "action": "Pick", "quantity":10, "itemBarcode":22332, "locationId":2}
                }
            }
        )

        self.assertTrue(ord.id==1,"error in id")
        self.assertTrue(ord.barcodeId == 2323, "barcode Id")
        self.assertTrue(ord.boxSize == BoxSize.S, "boxsize")
        self.assertTrue(ord.orderDate == datetime.date(2021,10,1), "datetime")
        self.assertTrue(ord.errorOccurred == False, "errorOccurred")
        self.assertTrue(ord.locked == True, "locked")
        self.assertTrue(ord.message == "test", "message")
        self.assertTrue(len(ord.orderActions) == 2)

if __name__ == '__main__':
    unittest.main()