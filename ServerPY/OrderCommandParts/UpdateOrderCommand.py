from  OrderCommandParts.Command  import  Command
from Order import  Order
from OrderAction import  OrderAction
from Item import  Item
from Database import db,loginedUsers
from threading import  Lock
lock = Lock()
class UpdateOrderCommand(Command):
    def __init__(self, currentUser, currentOrder,orderAction_dict_list:dict, errorOrderIDList:list):
        super().__init__(currentUser, currentOrder)
        self.data = orderAction_dict_list
        self.errorOrderIDList = errorOrderIDList




    def execute(self):
        #TODO: execute the command in user
        # remove those orders from user's pickedUpOrders
        # change the identity from lock to unlock
        # try to find all order in the errorOrder, errorOccurred = true for each
        self.interpretData()
        for order in self.currentUser.pickedUpOrders:
            order.locked = False
            db.session.add(order)

        # handle all error Orders.
        for orderID in self.errorOrderIDList:
            # try to search for the order
            result = self.find_order(orderID)
            if result == None:
                # means this order did not even exit before
                pass
            else:
                result.errorOccurred = True
                db.session.add(result)

        self.currentUser.pickedUpOrders = []
        db.session.commit()

    def interpretData(self):
        # TODO: interpret the data base on typs od command

        orderActionList = []
        with lock:
            for orderAction_dict in self.data:
                tempOrderAct = self.find_orderAction(orderAction_dict["id"]) # does not need to do a validation check
                                                                        #    check should have done previously in
                if tempOrderAct.id in self.errorOrderIDList:
                    continue # means it is one of the order with error
                elif tempOrderAct.status == False and bool( orderAction_dict["status"] ) == True:
                    # this allows that if user pass in the same order again, if written true in database, it will simply ignore
                    tempOrderAct.status = True
                    # calculate delta change on item table

                    # find correspond item
                    item = self.find_item(orderAction_dict["itemBarcode"])
                    if orderAction_dict["action"] == "Pick":
                        item.quantity -= int(orderAction_dict["quantity"])
                    else:
                        item.quantity += int(orderAction_dict["quantity"])
                    # commit to database
                    db.session.add(item)
                    db.session.add(tempOrderAct)
                    db.session.commit()
                else:
                    # make no changes
                    pass




    def OnRelease(self):
        # TODO: calls when the command is going to be release and delete
        pass

    def find_order(self,id: int):
        ords = Order.query.filter_by(id=id).first()
        if ords != None:
            return ords
        else:
            return None

    def find_orderAction(self,id: int):
        orderAction = OrderAction.query.filter_by(id=id).first()
        if orderAction != None:
            return orderAction
        else:
            return None
    def find_item(self,itemBarcode:int):
        item = Item.query.filter_by(itemBarcode=itemBarcode).first()
        if item != None:
            return item
        else:
            return None