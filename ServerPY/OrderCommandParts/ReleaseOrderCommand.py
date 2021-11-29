from OrderCommandParts.Command import  Command
from Database import  db,loginedUsers
from threading import  Lock
lock = Lock()
class ReleaseOrderCommand(Command):
    def __init__(self, currentUser, currentOrder):
        super().__init__(currentUser, currentOrder)




    def execute(self):
        #TODO: execute the command in user
        with lock:
            # get all orders in user's
            for order in self.currentUser.pickedUpOrders:
                order.locked = False
                db.session.add(order)
            # push updates to the database
            db.session.commit()
            self.currentUser.pickedUpOrders = []
            # remove the user form logined user list
            loginedUsers.remove(self.currentUser)




    def interpretData(self):
        # TODO: interpret the data base on typs od command
        pass
    def OnRelease(self):
        # TODO: calls when the command is going to be release and delete
        pass