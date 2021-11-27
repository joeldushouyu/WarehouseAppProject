from OrderCommandParts.Command import  Command
from Database import db
class GetOrderCommand(Command):
    def __init__(self, currentUser, correspondtOrder):
        super().__init__(currentUser, correspondtOrder)




    def execute(self):
        #TODO: execute the command in user
        self.currentUser.pickedUpOrders.append(self.correspondOrder)

    def interpretData(self):
        # TODO: interpret the data base on typs od command
        pass

    def OnRelease(self):
        # TODO: calls when the command is going to be release and delete
        # this methods only can calls by server's another running thread when is doing a constant check of cleaning Session()
        self.correspondOrder.locked = False;
        db.session.add(self.correspondOrder)
        db.session.commit()