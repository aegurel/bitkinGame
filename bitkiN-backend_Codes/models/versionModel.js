const mongoose = require('mongoose');
const schema = mongoose.Schema;

const VersionSchema = new schema({
    version:{
        type:Number
    },
    name:{
        type:String
    }
},{collection:'appversion',versionKey:false});

const Version = mongoose.model('Version',VersionSchema);
module.exports = Version;