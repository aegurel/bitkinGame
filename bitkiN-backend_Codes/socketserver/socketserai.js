const plantFeat = require('../plantfeatures/plantfeat');
const Plant = require('../models/plantModel');
const ChatM = require('../models/chatModel');

const healthEditor = async()=>{
    const water = plantFeat.retwat();
    const sun = plantFeat.retsun();
    const blow = plantFeat.retblow();
    let health = plantFeat.retheal();

    if(water<=700 && water>=300 && sun>=300 && sun<=700 && blow>=300 && blow<=700){
        if(health<1000){
            health+=1;
        }       
    }else if(water>700 && water<300 && sun<300 && sun>700 && blow<300 && blow>700){
        if(health>0){
            health-=1;
            if(health==0){
                await Plant.updateOne({isim:"bitki"},{expired:false});
            }
        }
    }
    plantFeat.plantEditing(water,sun,health,blow);
}
const plantEditor = ()=>{
    let water = plantFeat.retwat();
    let sun = plantFeat.retsun();
    let blow = plantFeat.retblow();
    let health = plantFeat.retheal();

    if(water > 19)
        water -= 20;
    else
        water-=19; 

    if(sun > 19)
        sun -= 20;
    else
        sun-=19;  
          
    if(blow > 19)
        blow -= 20;
    else
        blow-=19;    

    plantFeat.plantEditing(water,sun,health,blow);    
}
const plantExpTime = async ()=>{
    let expTime = plantFeat.retTime();
    expTime += 1;
    plantFeat.expTimeEditing(expTime);
    console.log(expTime);
    Plant.updateOne({isim:"bitki"},{experimentTime:expTime})
    .then()
    .catch(e=>console.log("bitki deney süresi hatası "+e));
}

const plantDatabase = async()=>{
    const water = plantFeat.retwat();
    const sun = plantFeat.retsun();
    const blow = plantFeat.retblow();
    let health = plantFeat.retheal();

    if(health==0){
        await Plant.updateOne({isim:"bitki"},{expired:false});
    }else{
        Plant.updateOne({isim:'bitki'},{waterInfo:water,sunInfo:sun,moistureInfo:blow,health:health})
        .then(console.log("tamamlandı"))
        .catch(e=>console.log("Bitki güncellenirken hata "+e));
    }
}

const chatDatabaseDelete = async()=>{
    await ChatM.deleteMany({});
}

module.exports={
    plantDatabase:plantDatabase,
    healthEditor:healthEditor,
    plantEditor:plantEditor,
    plantExpTime:plantExpTime,
    chatDatabaseDelete:chatDatabaseDelete
}