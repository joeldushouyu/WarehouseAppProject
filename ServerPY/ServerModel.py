
# the main class of the server

from datetime import date, datetime

import flask
from flask import request, abort, jsonify
from werkzeug.exceptions import HTTPException

from OrderCommandParts.LoginOrderCommand import LoginOrderCommand
from OrderCommandParts.GetOrderCommand import GetOrderCommand
from OrderCommandParts.UpdateOrderCommand import UpdateOrderCommand
from OrderCommandParts.ReleaseOrderCommand import  ReleaseOrderCommand
from User import User
from Database import app, loginedUsers, activeSessionList,db
from Item import  Item
from threading import Lock
from OrderAction import OrderAction
from Order import  Order
from LocationItem import  LocationItem
import uuid
from Session import  Session


lock = Lock()



orderActionOrderId = OrderAction.query.all() #TODO have thread update this
#app = Flask(__name__)
"""
app.config['SQLALCHEMY_DATABASE_URI'] = 'sqlite:///products.sqlite3'


db = SQLAlchemy(app)
db.create_all()"""



def determine_range(s:str):
    number = ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U",
              "V", "W", "X", "Y", "Z"]
    begin = number.index(s[0]) * 260 + number.index(s[1]) * 10 + 1
    end = begin + 9

    return(begin,end)

# ----------------------------------------
# Central error handler. 
# Converts abort() and unhandled application exceptions to JSON responses, and logs them
# ----------------------------------------
@app.errorhandler(Exception)
def handle_error(e):
    if isinstance(e, HTTPException):
        return jsonify(error=str(e)), e.code

    # An unhandled exception was thrown ... log the details
    data = request.json if request.json else request.data
    app.logger.error('%s %s\nRequest body: %s\n', request.method, request.full_path, data, exc_info=e)

    return jsonify(error=str(e)), 500


@app.before_request
def start_request():
    app.logger.warning('%s %s - Starting request processing', request.method, request.full_path)


@app.route("/login", methods=['POST'])
def login():
    userInfo_dict = request.get_json()

    try:
        username = userInfo_dict["accountName"]
        password = userInfo_dict["password"]


        # first to a check and see if the user is already in the activeSessionList
        value = [sess for sess in activeSessionList if isinstance( sess.action,  LoginOrderCommand) and sess.currentUser.accountName == username and sess.currentUser.password == password ]
        if(len(value) == 1):
            # each user should only have one Session() at a time
            return {"session" : value[0].currentUser.sessionId}
        else:
            # real login logic
            user =  User.query.filter_by(accountName=username).first()

            # will throw exectipin if user is none
            if user is None:
                abort(403, description="Invalid username or password")

            elif(user.password == password):
                # TODO pass
                with lock:
                    use = [u for u in loginedUsers if u.accountName == username]
                if len(use) == 0:
                    # means the current user is not logined

                    # check to see could the user be in the activeSessionList

                    newSession = uuid.uuid4() # random generate a session
                    with lock:
                        while len( [u for u in loginedUsers if u.sessionId == newSession ]) !=0:
                            newSession = uuid.uuid4()  # random generate a session


                    user.sessionId = newSession
                    # create a Session() with LoginOrderCommand,
                    loginSession = Session(newSession,LoginOrderCommand(user,None), date.today(), user)
                    # store session into the list
                    activeSessionList.append(loginSession)


                    #loginedUsers.append(user) # add user to logined list

                    # return the generate uuid
                    return {"session" : newSession}

                else:
                    # user already logined on other device
                    #TODO: what if connection is lost on the way back?
                    abort(405, description="Your account is login on other device")

            else:
                # password does not equal
                abort(403, description="Invalid username or password")
    except KeyError as e:
        abort(404, description="Incorrect format")
    except Exception as e:

        if(e.code ==405):
            abort(405, description="Your account is login on other device")
        elif(e.code == 403):
            abort(403, description="Invalid username or password")
        else:
            abort(404, description="Incorrect format")
    #return jsonify([product.to_dict() for product in products])


@app.route("/confirm", methods=['POST'])
def confirm_action():
    userInfo_dict = request.get_json()
    try:
        userUuid = userInfo_dict["session"]  # data of uuid
    except Exception as b:
        abort(400, "Incorrect format")

    try:

        # check if there is a Session() contains the same
        with lock:
            for sess in activeSessionList:
                if str(sess.sessionUUID) == userUuid:
                    sess.execute()
                    activeSessionList.remove(sess)
                    return flask.Response(200) # go out of loop,

            # if still here, means it did not find a session that match
            abort(404)

    except Exception as e:
        abort(404, description="Did not find valid session with the uuid provided")


@app.route("/islogin", methods=['POST'])
def is_login():
    userInfo_dict = request.get_json()

    try:
        username = userInfo_dict["accountName"]


        with lock:
            u_list = [u for u in loginedUsers if u.accountName == username]
            if(len(u_list) == 0):
                # means has not login
                abort(400, "User has not logined")

            else:
                return flask.Response(200)
    except Exception as e:
        if(e.code == 400):
            abort(400, "User has not logined")
        abort(404, description="Incorrect format information")


# check if the order is been pickedup by user
@app.route("/pickedUpOrder/<int:order_id>", methods=['POST'])
def is_pickup_by_user(order_id:int):
    userInfo_dict = request.get_json()
    try:
        userUuid = userInfo_dict["session"]
    except Exception:
        abort(404, "Incorrect format information")

    try:
        user = find_login_user(userUuid)
        #ord = find_order(order_id)

        if(len([o for o in user.pickedUpOrders if o.id == order_id]) == 1):
            return flask.Response(200)
        else:
            abort(404, "Not pickedup by user")
    except Exception as e:
        abort(404, "incorrect format")

@app.route("/readyToLogout", methods=['POST'])
def user_ready_to_logout():
    information = request.get_json()
    try:
        userUUID = information["session"]
    except Exception:
        abort(404, "incorrect format")

    try:
        user = find_login_user(userUUID)
        if len(user.pickedUpOrders) ==0:
            flask.Response(200)
        else:
            abort(404)
    except Exception as e:
        if(e.code == 404):
            abort(404, description="User is not ready to logout yet")
        else:
            abort(404, description="incorrect format or invalid message")
@app.route("/orderList",methods=['POST'])
def order():
    userInfo_dict = request.get_json()

    try:
        userUuid = userInfo_dict["session"]
        workingSection = userInfo_dict["section"]
    except Exception:
        abort(404,description="incorrect format")

    try:


        with lock:
            users = [u for u in loginedUsers if str(u.sessionId) == str(userUuid)]
        if(len(users) == 0):
            # no user login with this id
            abort(403, description="incorrect uuid")
        else:
            section = workingSection.split("-")
            range1 = determine_range(section[0])
            range2 = determine_range(section[1])

            orderActions = OrderAction.query.filter(OrderAction.locationId >= range1[0] ).all()
            filteredOrderActions = [o for o in orderActions if o.locationId <= range2[1]]
            filteredOrderActions = [ordAct for ordAct in filteredOrderActions if ordAct.status ==False]


            # give a list of order action correspond to it

            returnOrders = []
            for ordAct in filteredOrderActions:
                ord = Order.query.filter_by(id=ordAct.fromOrderID).all()
                for o in ord:
                    if o not in returnOrders and o.locked==False:
                        returnOrders.append(o)

            #message = []
            #length = len(filteredOrderActions)
            #for i in range(1, length+1):

               # message.append(filteredOrderActions[i-1].to_dict())
            x= flask.jsonify([ ords.to_dict() for ords in returnOrders])

            return flask.jsonify([ ords.to_dict() for ords in returnOrders])


    except Exception as e:
        if(e.code == 403):
            abort(403, description="incorrect uuid")
        abort(404, description="Incorrect format information")

@app.route("/pickOrder",methods=['POST'])
def pick_order():
    userInfo_dict = request.get_json()

    try:
        userUuid = userInfo_dict["session"]
        orderId = int(userInfo_dict["orderID"])
        assignBarcode = int(userInfo_dict["assignedBarcode"])
    except Exception as e:
        abort(404, description="incorrect format")

    try:

        #TODO: could user pick up? check for twice action
        # first check if the server is already
        # check and see if the order is already in a Session() in the activeSessionList
        ordSess = [ses for ses in activeSessionList if isinstance(ses.action, GetOrderCommand) and ses.sessionUUID ==userUuid ]
        if(len(ordSess)) == 1:
            # means the request has already received by user,
            # return information it need
            data = ordSess[0].action.correspondOrder.to_dict_jsonify()
            return data
        else:
            with lock:
                # confirm if this order is still in order list and have not been locked
                matchOrder = find_order(orderId)
                if matchOrder != None:
                    if (matchOrder.locked == True):
                        abort(404, description="This order has already been picked up by someone else")
                    else:
                        # means matchOrder is not locked and has not been picked up



                        # assign the following order to the user
                        user = find_login_user(userUuid)
                        if (user == None):
                            # means a invalid session,
                            abort(403)
                        else:

                            matchOrder.locked = True;
                            if(int(matchOrder.barcodeId) == 0):
                                # means was not assigned previously
                                matchOrder.barcodeId = assignBarcode

                            # a valid user with valid order id
                            getOrderSession = Session(userUuid, GetOrderCommand(user, matchOrder),date.today(),user )
                            activeSessionList.append(getOrderSession)
                            db.session.add(matchOrder)  # change to database

                            matchOrder.load_orderActions()

                            db.session.commit()
                            data = matchOrder.to_dict_jsonify()
                            return data

                else:
                    # means there is not a order correspond to this
                    abort(404)  # no id associate with this order


    except Exception as e:
        if(e.code == 403):
            abort(403, description="incorrexct uuid")
        elif(e.code == 404 and e.message == "This order has already been picked up by someone else"):
            abort(404, description="This order has already been picked up by someone else")
        else:
            abort(404, description="Incorrect format information")





@app.route("/updateOrder",methods=['POST'])
def update_order():
    info_list = request.get_json()
    update_order_list_dict = []

    try:
        userUUID = info_list["session"]
        update_order_list_dict = info_list["OrderList"]
        print(update_order_list_dict)



    except Exception as e:
        abort(404, description="incorrect format")

    try:

        # first, validate and see if it is a valide user UUID session
        user =  find_login_user(userUUID)
        if user == None:
            # not valid uuid
            abort(403)
        else:
            updateSess = [ses for ses in activeSessionList if isinstance(ses.action, UpdateOrderCommand) and ses.sessionUUID ==userUUID ] #TODO: modify logic here
            if len(updateSess) == 1:
                # means it already has a Session()
                # in other word,the user with the same UUID already requested before
                message =  {"ErrorOrderID":updateSess[0].action.errorOrderIDList}
                #activeSessionList.remove(updateSess[0]) # only for debug
                return  message # indicate that the server has already received the request
            else:
                tempOrderAction_dict_list = []
                errorOrderIDList = []
                for i in range(len(update_order_list_dict)):

                    # find the order
                    with lock:
                        # do a validation check, ensure both order ID and orderAction ID correctly present on the server
                        ord =  find_order( update_order_list_dict[i]["id"])
                        if ord == None :
                            # means the user pass a invalid order ID
                            # simply ignore this specifc order update
                            errorOrderIDList.append(update_order_list_dict[i]["id"])
                            continue
                        elif 1 != len([orde for orde in user.pickedUpOrders if orde.id == update_order_list_dict[i]["id"]]):
                            # means for some reason, Server was not notified that this user actually picked up this order
                            # since someone else could have already pick up this order, server will ignore this specific order update
                            errorOrderIDList.append(update_order_list_dict[i]["id"])
                            continue
                        else:
                            tempOrderAction_dict = update_order_list_dict[i]["OrderActions"]
                            for orderActDict in tempOrderAction_dict:
                                orderAct = find_orderAction(orderActDict["id"])
                                if orderAct == None:
                                    # means did not find an OrderAction with the correspond id, inconsistent with data
                                    errorOrderIDList.append(update_order_list_dict[i]["id"])
                                    break
                                elif (orderActDict["action"] != "Supply" and orderActDict["action"]!= "Pick") or orderActDict["action"]!= str(orderAct.action)  :

                                    # incorrect action type
                                    errorOrderIDList.append(update_order_list_dict[i]["id"])
                                    break
                                elif int(orderActDict["quantity"]) <= 0:
                                    # incorrect quantity value
                                    errorOrderIDList.append(update_order_list_dict[i]["id"])
                                    break
                                else:
                                    #TODO: Also verify each item id?
                                    tempOrderAction_dict_list.append(tempOrderAction_dict[0])
                updateSession = Session(userUUID, UpdateOrderCommand(user, None, tempOrderAction_dict_list, errorOrderIDList), date.today(), user)
                activeSessionList.append(updateSession)
                return {"ErrorOrderID":errorOrderIDList}





    except Exception as e:
        if(e.code == 403):
            abort(403, description="incorrect uuid")
        else:
            abort(404, description="incorrect format or not valid data")


@app.route("/itemDetail/<int:location_id>",methods=['POST'])
def item_detail(location_id:int):
    userInfo_dict = request.get_json()

    try:
        userUuid = userInfo_dict["session"]
        orderID = int(location_id)
    except Exception:
        abort(404,description="incorrect format")

    try:

        # check to see if user has logined
        with lock:
            user = find_login_user(userUuid)
            if user == None:
                abort(403)
            else:

                loc = LocationItem.query.filter_by(id=location_id).first()
                if(loc == None):
                    abort(404)
                else:
                    item = Item.query.filter_by(itemBarcode =loc.itemBarcode)
                    if item == None:
                        abort(500)
                    else:

                        return {"ItemDetail":item.to_dict()}


    except Exception as e:
        if(e.code == 403):
            abort(403, description="incorrect uuid")
        abort(404, description="Incorrect format information")


@app.route("/notificationList",methods=['POST'])
def notification_list():
    datetime.strptime("2021 10 01","%Y %m %d")
    datetime.datetime(2021, 10, 1, 0, 0)


@app.route("/logout",methods=['POST'])
def logout_user():
    info = request.get_json()
    try:
        userUUID= info["session"]
    except Exception:
        abort(404, "incorrect format")

    try:

        user = find_login_user(userUUID)

        if(user == None):
            return flask.Response(200) # just return and tell user that it has already logout
        else:
            releaseSession = Session(user, ReleaseOrderCommand(user, None),date.today(), user )

            # this time, just execute the action
            releaseSession.execute()
            return flask.Response(200)

    except Exception as e:
        abort(500, "Unknow error occurs with server")




    # now try to update the

def find_login_user(uuid:str):
    loginUser = [log for log in loginedUsers if str(log.sessionId) == str(uuid)]
    if(len(loginUser) == 1):
        return loginUser[0]
    else:
        return None

def find_order(id:int):
    ords = Order.query.filter_by(id=id).first()
    if ords != None:
        return ords
    else:
        return None
def find_orderAction(id:int):
    orderAction = OrderAction.query.filter_by(id=id).first()
    if orderAction != None:
        return orderAction
    else:
        return None
#TODO
# need another thread to regularly clear out the Command /activeUserSession, also for logined user??


if __name__ == '__main__':
    app.run(host='localhost', port=5000, debug=True)

