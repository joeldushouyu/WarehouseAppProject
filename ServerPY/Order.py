import datetime
from Enumerables import BoxSize
class Order():
    def __init__(self, databaseID:int, barcodeID:int, boxSize: BoxSize, orderDate:datetime,message:str):
        self.id = databaseID
        self.barcodeId = barcodeID
        self.boxSize = boxSize
        self.orderDate = orderDate
        self.errorOccured  = False
        self.locked = False
        self.orderActions = []   # should hold list of OrderAction
        self.message = message  # description and message come with the

