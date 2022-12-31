const express = require('express');
const User = require('../models/userModel');
const Plant = require('../models/plantModel');
const Version = require('../models/versionModel');
const ChatM = require('../models/chatModel');
const authMiddleware = require('../middleware/authMiddleware');
const adminMiddleware = require('../middleware/adminMiddleware');

const router = express.Router();

router.get('/:idFirebase',async(req,res)=>{
    console.log(req.params.idFirebase);  
    const kisi = await User.find({idFirebase:req.params.idFirebase},{username:1,papel:1,noAds:1,_id:0});
    res.json(kisi);
});

router.get('/papel/:idFirebase',async(req,res)=>{
    console.log(req.params.idFirebase);  
    const kisi = await User.find({idFirebase:req.params.idFirebase},{papel:1,_id:0});
    res.json(kisi);
});

router.get('/goodslist/info',async(req,res)=>{
    let iyiler = await User.find({},{username:1,isGood:1,_id:0},{limit:20}).sort({isGood:-1});
    console.log("ben");
    res.json(iyiler);
});

router.get('/badslist/info',async(req,res)=>{
    const kotuler = await User.find({},{username:1,isBad:1,_id:0},{limit:20}).sort({isBad:-1});
    res.json(kotuler);
});

router.get('/goodslist/update/:id',async(req,res)=>{  
    await User.findOneAndUpdate({idFirebase:req.params.id},{$inc:{isGood:3}},{useFindAndModify:false});
    res.json("başarılı");
});

router.get('/badslist/update/:id',async(req,res)=>{ 
    await User.findOneAndUpdate({idFirebase:req.params.id},{$inc:{isBad:3}},{useFindAndModify:false});
    res.json("başarılı");
});

router.get('/app/version',async(req,res)=>{
    const version = await Version.find({},{version:1,_id:0});
    res.json(version);
});

router.get('/app/experimentTime',async(req,res)=>{
    const expTime = await Plant.find({},{_id:0,experimentTime:1});
    res.json(expTime);
});

router.get('/app/sonuc',async(req,res)=>{
    const result = await Plant.find({},{_id:0,sonuc:1});
    res.json(result);
});

router.get('/app/chat',async(req,res)=>{
    const result = await ChatM.find({},{_id:0,chatMesajj:1,username:1});
    res.json(result);
});

router.get('/two/papel/:id',async(req,res)=>{
    await User.findOneAndUpdate({idFirebase:req.params.id},{$inc:{papel:200}},{userFindAndModify:false});
    res.json("");
});

router.get('/five/papel/:id',async(req,res)=>{
    await User.findOneAndUpdate({idFirebase:req.params.id},{$inc:{papel:500}},{userFindAndModify:false});
    res.json("");
});

router.get('/thousand/papel/:id',async(req,res)=>{
    await User.findOneAndUpdate({idFirebase:req.params.id},{$inc:{papel:1000}},{userFindAndModify:false});
    res.json("");
});

router.get('/tthousand/papel/:id',async(req,res)=>{
    await User.findOneAndUpdate({idFirebase:req.params.id},{$inc:{papel:2000}},{userFindAndModify:false});
    res.json("");
});

router.get('/fthousand/papel/:id',authMiddleware,async(req,res)=>{
    await User.findOneAndUpdate({idFirebase:req.params.id},{$inc:{papel:4000}},{userFindAndModify:false});
    res.json("");
});

router.get('/app/noads/:id',async(req,res)=>{
    await User.findOneAndUpdate({idFirebase:req.params.id},{noAds:true},{userFindAndModify:false});
    res.json("");
});

router.post('/user/kayit',async(req,res)=>{
    try{
        const kayitlanacakUser = new User(req.body);
        const sonuc = await kayitlanacakUser.save();
        res.json(sonuc);
    }catch(e){
        console.log("hata oluştu "+e);
    }
});

/*router.post('/admin/version',[authMiddleware,adminMiddleware],async(req,res)=>{
    try{
        const newVersion = new Version(req.body);
        const sonuc = await newVersion.save();
        res.json(sonuc); 
    }catch(e){
        console.log("version kayıt hatası "+e);
    }
});*/


/*router.patch('/admin/versionUpdate',[authMiddleware,adminMiddleware],async(req,res)=>{
    await Version.updateOne({name:'myVersion'},{$inc:{version:1}});
});*/

module.exports = router;
