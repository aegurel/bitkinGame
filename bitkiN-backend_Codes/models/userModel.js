const mongoose = require('mongoose');
const schema = mongoose.Schema;


const UserSchema = new schema({
    idFirebase:{
        type:String,
        required:true,
        trim:true,
        unique:true
    },
    username:{
        type:String,
        required:true,
        unique:true,
        trim:true,
        minLength:3,
        maxLength:20
    },
    email:{
        type:String,
        required:true,
        trim:true,
        unique:true,
        lowercase:true,
    },
    isAdmin:{
        type:Boolean,
        default:false
    },
    isBad:{
        type:Number,
        default:0
    },
    isGood:{
        type:Number,
        default:0
    },
    papel:{
        type:Number,
        default:300
    },
    noAds:{
        type:Boolean,
        defaulf:false
    }
},{collection:'userList',versionKey:false});

const User = mongoose.model('User',UserSchema);
module.exports = User;

