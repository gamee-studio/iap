# What

Integration module for implementing in app purchase for Unity 3d

## How To Install

Add the following lines

- for newest update

```csharp
"com.gamee.iap": "https://github.com/gamee-studio/iap.git?path=Assets/_Root",
"com.snorlax.simplejson": "https://github.com/snorluxe/SimpleJSON.git?path=Assets/_Root",
```

- for excactly version

```csharp
"com.gamee.iap": "https://github.com/gamee-studio/iap.git?path=Assets/_Root#1.0.2",
"com.snorlax.simplejson": "https://github.com/snorluxe/SimpleJSON.git?path=Assets/_Root",
```

To `Packages/manifest.json`

## Usage

![image](https://user-images.githubusercontent.com/44673303/158574310-a4fcf82d-4bd3-494a-93b2-574db8cdff7a.png)

#### _SETTING_

1. Auto Init : Always true, when game starting IAPManager auto initialize

2. Skus : List of product id
    - Id : Default id use when override mark false
    - Android Id : Product id use for android platfom when override mark true
    - iOS Id : Product id use for ios platfom when override mark true
    - Product Type:
        - Consumable : (pay everytime)
          A consumable In-App Purchase must be purchased every time the user downloads it. One-time services, such as fish food in a fishing app, are usually implemented as
          consumables.
        - Non-Consumable : (one time payment)
          Non-consumable In-App Purchases only need to be purchased once by users. Services that do not expire or decrease with use are usually implemented as non-consumables, such
          as new race tracks for a game app.
        - Subscriptions : (will deduct money from your credit card on a cycle complete)
          Subscriptions allow the user to purchase updating and dynamic content for a set duration of time. Subscriptions renew automatically unless the user opts out, such as
          magazine subscriptions.

3. Button Generate Script:
    - Generate script contains all method purchase for all skus definition in IapSettings.asset
    - Then you can call method inside ProductImpl class to make specific item purchase

```c#
ProductImpl.PurchaseRemoveads(); // ex call purchase remove ads item
```

4 You need to attach your custom event callback (purchase success and purchase faild) manual by following way, IAPManager is initialized automatically so don't worry about null
error

```c#

IAPManager.Instance.OnPurchaseSucceedEvent += YourHandlePurchaseSucceedEvent;
IAPManager.Instance.OnPurchaseFailedEvent += YourHandlePurchaseFailedEvent;

private void YourHandlePurchaseSucceedEvent(PurchaseEventArgs product)
{
    switch (product.purchasedProduct.definition.id)
    {
        case "com.larnten.removeads":
            break;
        // TO_DO
    }
}

private void YourHandlePurchaseFailedEvent(string obj)
{
    // TO_DO        
}

```

