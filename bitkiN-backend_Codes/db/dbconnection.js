const dotenv = require('dotenv').config();
const plantfet = require('../plantfeatures/plantfeat');
const mongoose = require('mongoose');
mongoose.connect(process.env.MONGODB_CONNECTION_STRING,{useCreateIndex:true,useNewUrlParser:true,useUnifiedTopology:true})
.then(()=>plantfet.plantfeatu())
.catch(h=>console.log(h));
