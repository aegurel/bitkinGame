const plantModel = require('../models/plantModel');
const plantfeatu = require('../plantfeatures/plantfeat');
const User = require('../models/userModel'); 
const ChatM = require('../models/chatModel');

let plant={
    message:"",
    valueWater:"",
    valueSun:"",
    valueBlower:"",
    valueHealth:"",
    chatUsername:"",
    specialChatUsername:"",
    chatMessage:"",
    specialChatMessage:""   
}

const plantGiris = async()=>{
    
    const myPlant = await plantModel.find({},{expired:1});
        const plantBilgi = {
            expired:myPlant[0].expired,
            waterInfo:plantfeatu.retwat(),
            moistureInfo:plantfeatu.retblow(),
            sunInfo:plantfeatu.retsun(),
            healthInfo:plantfeatu.retheal(),
            experimentTime:plantfeatu.retTime()
        }
       const json = JSON.stringify(plantBilgi);
       return new Promise((resolve,reject)=>{
           setTimeout(()=>{
               resolve(json);
           },2000);
       });
    }

const sendingPlants = async(mesaj)=>{
    
let waterVal=plantfeatu.retwat(); 
let sunVal=plantfeatu.retsun();;
let healthVal=plantfeatu.retheal(); 
let blowVal=plantfeatu.retblow();

    if(mesaj == 'water'){
        if(waterVal>700){
            healthVal-=1;
        }
        waterVal+=1;
        blowVal+=1;      
    }
    else if(mesaj=='sun'){
        if(sunVal>700){
            healthVal-=1;
        }
        waterVal-=1;
        blowVal-=1;
        sunVal+=1;   
    }else if(mesaj=='blow'){
        if(blowVal>650){
            healthVal-=1;
        }
        waterVal-=1;
        blowVal+=1;        
    }
    if(healthVal == 0){
        await plantModel.updateOne({isim:"bitki"},{expired:false});
    }
    return new Promise((resolve,reject)=>{
        setTimeout(()=>{
            resolve(sendingPlant(waterVal,sunVal,blowVal,healthVal));
        },100);
    }); 
       
}

const sendingChats=async(username,chatMes)=>{
    const chat={
        username:username,
        chatMesajj:chatMes
    };
    const kayitChat = new ChatM(chat);
    const sonuc = await kayitChat.save();

    plant.valueBlower = null;
    plant.valueHealth = null;
    plant.valueSun = null;
    plant.valueWater = null;
    plant.specialChatUsername = null;
    plant.specialChatMessage = null;
    plant.message = 'Chat';
    plant.chatUsername=username;
    plant.chatMessage=chatMes;

    const sendingChat = JSON.stringify(plant);
    return new Promise((resolve,reject)=>{
        setTimeout(()=>{
            resolve(sendingChat);
        },2000)
    });
}
const sendingSpecialChats = async(username,specialChatMes,userId)=>{
    await User.updateOne({idFirebase:userId},{$inc:{papel:-100}});

    plant.valueBlower = null;
    plant.valueHealth = null;
    plant.valueSun = null;
    plant.valueWater = null;
    plant.chatMessage = null;
    plant.message = 'SpecialChat';
    plant.specialChatUsername=username;
    plant.specialChatMessage=specialChatMes;

    const sendingSpecialChat = JSON.stringify(plant);

    return new Promise((resolve,reject)=>{
        setTimeout(()=>{
            resolve(sendingSpecialChat);
        },2000);
    });
}
const sendingPlant=(wat,sun,blow,heal)=>{
    plantfeatu.plantEditing(wat,sun,heal,blow);

    plant.valueBlower = blow;
    plant.valueHealth = heal;
    plant.valueSun = sun;
    plant.valueWater = wat;
    plant.message = 'plant';
    plant.chatUsername=null;
    plant.chatMessage=null;
    plant.specialChatMessage = null;
    plant.specialChatUsername = null;

    const plantInfo = JSON.stringify(plant);
    return plantInfo;
}

module.exports={
    plantGiris:plantGiris,
    sendingPlants: sendingPlants,
    sendingChats:sendingChats,
    sendingSpecialChats:sendingSpecialChats
}
