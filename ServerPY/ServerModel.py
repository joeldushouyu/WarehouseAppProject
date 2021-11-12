



# the main class of the server
import flask

loginedUsers = []
notificationList = []
activeSessionList = [] # list hold all of Session()



from flask import Flask, request, abort, jsonify, Response
from werkzeug.exceptions import HTTPException
import json
from User import User
import time
from flask_sqlalchemy import SQLAlchemy
from Database import db,app
from threading import Lock
import uuid


lock = Lock()

#app = Flask(__name__)
"""
app.config['SQLALCHEMY_DATABASE_URI'] = 'sqlite:///products.sqlite3'


db = SQLAlchemy(app)
db.create_all()"""

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


        user =  User.query.filter_by(accountName=username).first()

        # will throw exectipin if user is none
        if(user.password == password):
            # TODO pass
            with lock:
                use = [u for u in loginedUsers if u.accountName == username]
            if len(use) == 0:
                # means the current user is not logined


                newSession = uuid.uuid4() # random generate a session
                with lock:
                    while len( [u for u in loginedUsers if u.sessionId == newSession ]) !=0:
                        newSession = uuid.uuid4()  # random generate a session


                user.sessionId = newSession
                loginedUsers.append(user) # add user to logined list

                # return the generate session
                return {"session" : newSession}

            else:
                # user already logined on other device

                abort(405, description="Your account is login on other device")


        else:
            # password does not equal
            abort(403, description="Invalid username or password")

    except Exception as e:
        abort(403, description="Invalid username or password")
    #return jsonify([product.to_dict() for product in products])



@app.route("/islogin", methods=['POST'])
def is_login():
    userInfo_dict = request.get_json()


    username = userInfo_dict["accountName"]
    password = userInfo_dict["password"]

    with lock:
        u_list = [u for u in loginedUsers if u.accountName == username]
        if(len(u_list) == 0):
            # means has not login
            abort(404, "User has not logined")

        else:
            return flask.Response(200)


#TODO
# need another thread to regularly clear out the Command /activeUserSession, also for logined user??

if __name__ == '__main__':
    app.run(host="0.0.0.0", debug=True)

