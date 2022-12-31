require('./models/plantModel');
require('./plantfeatures/plantfeat');
require('./db/dbconnection');

const dotenv = require('dotenv').config();
const plantFeature = require('./plantfeatures/plantfeat');
const socketser = require('./socketserver/socketser');
const ai = require('./socketserver/socketserai');
const wso = require('ws');
const express = require('express');
const userRouter = require('./router/userRouter');
const http = require('http');
const { Promise } = require('mongoose');
const { debug } = require('console');

//plantFeature.plantfeatu();
const app = express();
const server = http.createServer(app);

let val;
var CLIENTS=[];

app.use(express.json());
app.use(express.urlencoded({extended: true}));

setInterval(ai.healthEditor,1740000);
setInterval(ai.plantDatabase,1860000);
setInterval(ai.plantEditor,1800000);
setInterval(ai.plantExpTime,86400000);
setInterval(ai.chatDatabaseDelete,10000);

app.use('/careme/api/info',userRouter);

  app.get('/', (req,res)=>{
    res.send("Hello from express server.")
})

server.listen(process.env.PORT,()=>console.log("sea"));

const wss = new wso.Server({server:server});


wss.on('connection',(ws)=>{
  CLIENTS.push(ws);
    ws.on('message',(data)=>{
      let json = JSON.parse(data);
      console.log(json.message);
        if(json.message == 'Chat'){
          socketser.sendingChats(json.username,json.chatMessage).then(sonuc=>{
            val=sonuc;
            sendAll(val);
          });
        }
        else if(json.message=='SpecialChat'){
          socketser.sendingSpecialChats(json.username,json.chatSpecialMessage,json.userId).then(sonuc=>{
            val = sonuc;
            sendAll(val);
          });          
        }
        else if(json.message == 'water'||json.message == 'sun'||json.message == 'blow'){
          socketser.sendingPlants(json.message).then(sonuc=>{
            val=sonuc;
            console.log(val);
            sendAll(val);
          });
        }
        else if(json.message == 'giris'){
          socketser.plantGiris().then(plant=>{
            val=plant;
            console.log(plant);
            ws.send(val);
          });
        }
    });

    ws.on('close',function(){
      //CLIENTS.pop(ws);
    });
});

function sendAll(message) {
  for (var i=0; i < CLIENTS.length; i++) {
     CLIENTS[i].send(message); // broadcast messages to everyone including sender
  }
}



