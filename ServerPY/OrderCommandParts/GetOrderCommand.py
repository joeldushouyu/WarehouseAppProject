from OrderCommandParts.Command import  Command
from Database import db
class GetOrderCommand(Command):
    def __init__(self, currentUser, correspondtOrder):
        super().__init__(currentUser, correspondtOrder)




    def execute(self):

        self.currentUser.pickedUpOrders.append(self.correspondOrder)

    def interpretData(self):

        pass

    def OnRelease(self):

        # this methods only can calls by server's another running thread when is doing a constant check of cleaning Session()
        self.correspondOrder.locked = False;
        db.session.add(self.correspondOrder)
        db.session.commit()