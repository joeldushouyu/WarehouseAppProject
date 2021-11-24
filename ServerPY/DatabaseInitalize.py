# To use this,
# pip install flask_sqlalchemy

from flask import Flask, request, abort, jsonify, Response
from werkzeug.exceptions import HTTPException
import json
import time
from flask_sqlalchemy import SQLAlchemy

app = Flask(__name__)
app.config['SQLALCHEMY_DATABASE_URI'] = 'sqlite:///test.sqlite3'
db = SQLAlchemy(app)

from datetime import datetime


class Post(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    title = db.Column(db.String(80), nullable=False)
    body = db.Column(db.Text, nullable=False)
    pub_date = db.Column(db.DateTime, nullable=False,
        default=datetime.utcnow)

    category_id = db.Column(db.Integer, db.ForeignKey('category.id'),
        nullable=False)
    category = db.relationship('Category',
        backref=db.backref('posts', lazy=True))

    def __repr__(self):
        return '<Post %r>' % self.title


class Category(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    name = db.Column(db.String(50), nullable=False)

    def __repr__(self):
        return '<Category %r>' % self.name

db.create_all() # Create tables from model classes


py = Category(name='Python')
Post(title='Hello Python!', body='Python is pretty cool', category=py)
p = Post(title='Snakes', body='Ssssssss')
py.posts.append(p)
db.session.add(py)
db.session.commit()
print(py.posts)
print(Post.query.with_parent(py).filter(Post.title != 'Snakes').all())