const mongoose = require('mongoose');
const schema = mongoose.Schema;

const chatSchema = new schema({
    username:{
        type:String
    },
    chatMesajj:{
        type:String
    }
},{collection:'chatList',versionKey:false});

const ChatM = mongoose.model('ChatM',chatSchema);
module.exports = ChatM;