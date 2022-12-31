const mongoose = require('mongoose');
const schema = mongoose.Schema;

const plantSchema = new schema({
    expired:{
        type:Boolean
    },
    waterInfo:{
        type:Number
    },
    sunInfo:{
        type:Number
    },
    moistureInfo:{
        type:Number
    },
    health:{
        type:Number
    },
    experimentTime:{
        type:Number
    },
    sonuc:{
        type:String
    }
},{collection:'plantlist',versionKey:false});

const Plant = mongoose.model('Plant',plantSchema);

module.exports = Plant;