using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public static class IAPConstants {
    
    public const string KEY_CURRENCY_ID = "KY";
    public const string STAR_CURRENCY_ID = "SR";

    public enum IAPProduct {

        BUY_2_KEYS,
        BUY_5_KEYS,
        BUY_5_STARS,
        BUY_10_STARS,
        BUY_30_STARS,
        BUY_50_STARS,
        BUY_100_STARS,
        BUY_300_STARS
    }

    public static IAPProduct? GetIAPProductFromProductId(string pProductId){
        foreach(KeyValuePair<IAPProduct,IAPProductData> kvp in PRODUCT_DATAS){
            if(kvp.Value.productId.Equals(pProductId,StringComparison.Ordinal)){
                return kvp.Key;
            }
        }
        return null;
    }

    public class IAPProductData {

        string _productId;

        public string productId {
            get {
                return _productId;
            }
        }

        string _currency;

        public string currency {
            get {
                return _currency;
            }
        }

        int _amount;

        public int amount {
            get {
                return _amount;
            }
        }

        public IAPProductData(string pProductId, string pCurrency, int pAmount) {
            _productId = pProductId;
            _currency = pCurrency;
            _amount = pAmount;
        }
    }



    private static readonly Dictionary<IAPProduct, IAPProductData> PRODUCT_DATAS = 
        new Dictionary<IAPProduct, IAPProductData>(){

        { IAPProduct.BUY_2_KEYS, new IAPProductData("oneapp.buykey.2",KEY_CURRENCY_ID,2) },
        { IAPProduct.BUY_5_KEYS, new IAPProductData("oneapp.buykey.5",KEY_CURRENCY_ID,2) },
        { IAPProduct.BUY_5_STARS, new IAPProductData("oneapp.buyhint.5",STAR_CURRENCY_ID,5) },
        { IAPProduct.BUY_10_STARS, new IAPProductData("oneapp.buyhint.0",STAR_CURRENCY_ID,10) },
        { IAPProduct.BUY_30_STARS, new IAPProductData("oneapp.buyhint.1",STAR_CURRENCY_ID,30) },
        { IAPProduct.BUY_50_STARS, new IAPProductData("oneapp.buyhint.2",STAR_CURRENCY_ID,50) },
        { IAPProduct.BUY_100_STARS, new IAPProductData("oneapp.buyhint.3",STAR_CURRENCY_ID,100) },
        { IAPProduct.BUY_300_STARS, new IAPProductData("oneapp.buyhint.4",STAR_CURRENCY_ID,300) }
    };

    public static string GetProductId(this IAPProduct self) {
        IAPProductData data;
        if(PRODUCT_DATAS.TryGetValue(self, out data)){
            return data.productId;
        }
        return null;
    }

    public static string GetCurrency(this IAPProduct self) {
        IAPProductData data;
        if (PRODUCT_DATAS.TryGetValue(self, out data)) {
            return data.currency;
        }
        return null;
    }

    public static int GetAmount(this IAPProduct self) {
        IAPProductData data;
        if (PRODUCT_DATAS.TryGetValue(self, out data)) {
            return data.amount;
        }
        return 0;
    }

    public static IAPProductData GetProductData(this IAPProduct self) {
        IAPProductData data;
        if (PRODUCT_DATAS.TryGetValue(self, out data)) {
            return data;
        }
        return null;
    }
}


