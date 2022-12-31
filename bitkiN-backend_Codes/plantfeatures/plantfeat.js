const Plant = require('../models/plantModel');

let waterVal=1;
let sunVal ;
let healthVal;
let blowVal;
let expTime;

const plantfeatu = async()=>{
    const plant = await Plant.find({isim:'bitki'});
    return new Promise((resolve,reject)=>{
        setTimeout(()=>{
            expTime = plant[0].experimentTime;
            sunVal = plant[0].sunInfo;
            waterVal = plant[0].waterInfo;
            blowVal = plant[0].moistureInfo;
            healthVal = plant[0].health;
        },100);
    }); 
}

const plantEditing = (wat,sun,heal,blow)=>{
    waterVal = wat;
    sunVal=sun;
    healthVal = heal;
    blowVal=blow
}
const expTimeEditing = (et)=>{
    expTime = et;
}
const retwat = ()=>{
    return waterVal;
}
const retsun = ()=>{
    return sunVal;
}
const retblow = ()=>{
    return blowVal;
}
const retheal = ()=>{
    return healthVal;
}
const retTime= ()=>{
    return expTime;
}
module.exports={
    plantfeatu:plantfeatu,
    plantEditing:plantEditing,
    expTimeEditing:expTimeEditing,
    retwat:retwat,
    retsun:retsun,
    retblow:retblow,
    retheal:retheal,
    retTime:retTime
}


