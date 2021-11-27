
from flask import Flask, request, abort, jsonify, Response
from flask_sqlalchemy import  SQLAlchemy,orm


app = Flask(__name__)
app.config['SQLALCHEMY_DATABASE_URI'] = 'sqlite:///products.sqlite3'

SQLALCHEMY_TRACK_MODIFICATIONS = False
db = SQLAlchemy(app,session_options={"expire_on_commit": False})
db.create_all()

loginedUsers = []  # store logined users
notificationList = []
activeSessionList = [] # list hold all of Session()




