import unittest


from Enumerables import ActionType
from orderaction import  OrderAction


class OrderActionTestCase(unittest.TestCase):


    def test_to_dict(self):
        orderACtion = OrderAction(10, 2031, ActionType(1), 10)
        self.assertEqual(orderACtion.to_dict(), {"id": "10", "fromOrderID":"2031", "action": "Pick", "quantity":"10"},"Error in to_dict()")

if __name__ == '__main__':
    unittest.main()