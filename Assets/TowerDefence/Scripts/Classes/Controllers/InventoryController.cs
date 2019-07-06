using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryController {

    public static InventoryController instance;
    
    private GUIText getCount0;
    private GUIText getCount1;
    private GUIText getCount2;
    
    private GUIText getPrice0;
    private GUIText getPrice1;
    private GUIText getPrice2;
    
    private GUIButton buy0;
    private GUIButton buy1;
    private GUIButton buy2;
    
    private GUIText name;
    private GUIText description;

    private GUIObject select;

    private List <string> items;

    private List <GUIImage> itemCounts; 

    private List <GUIObject> itemObjects;

    private int currentStart = 0;

    private bool isMoving;
    private int selectedIndex = -100;

    public static string GetItemIcon (string item) {
        
        if (item.Contains ("AcidFog")) {
            
            return "Textures/Inventory/AcidFog";
        }
        
        if (item.Contains ("Freezing")) {
            
            return "Textures/Inventory/Freezing";
        }

        if (item.Contains ("Bomb")) {
            
            return "Textures/Inventory/Bomb";
        }

        if (item.Contains ("Gold")) {
            
            return "Textures/Inventory/Gold";
        }

        if (item.Contains ("Heal")) {
            
            return "Textures/Inventory/Heal";
        }

        if (item.Contains ("Power")) {
            
            return "Textures/Inventory/Power";
        }

        return "Textures/Inventory/Gold";
    }
    
    public static string GetItemIconGameplay (string item) {
        
        if (item.Contains ("AcidFog")) {
            
            return "Textures/Gameplay/AcidFog";
        }
        
        if (item.Contains ("Freezing")) {
            
            return "Textures/Gameplay/Freezing";
        }

        if (item.Contains ("Bomb")) {
            
            return "Textures/Gameplay/Bomb";
        }

        if (item.Contains ("Gold")) {
            
            return "Textures/Gameplay/Gold";
        }

        if (item.Contains ("Heal")) {
            
            return "Textures/Gameplay/Heal";
        }

        if (item.Contains ("Power")) {
            
            return "Textures/Gameplay/Power";
        }

        return "Textures/Gameplay/Gold";
    }

    private string GetItemName (string item) {

        return Settings.GetText (item + "ItemName");
    }
    private string GetItemBottomName (string item) {

        return Settings.GetText (item + "ItemBottomName");
    }
    
    private string GetItemDescription (string item) {

        return Settings.GetText (item + "ItemDescription");
    }

    private List <int> GetItemCountPerOrder (string item) {
        
        if (item.Contains ("AcidFog")) {

            return new List<int> {1, 5, 25 };
        }

        if (item.Contains ("Freezing")) {

            return new List<int> {1, 5, 25 };
        }

        if (item.Contains ("Bomb")) {

            return new List<int> {1, 4, 24 };
        }

        if (item.Contains ("Gold")) {

            return new List<int> {1, 3, 23 };
        }

        if (item.Contains ("Heal")) {

            return new List<int> {1, 5, 25 };
        }

        if (item.Contains ("Power")) {

            return new List<int> {1, 7, 27 };
        }

        return new List<int> {-1, -2, -3 };
    }

    private List <int> GetItemPricePerOrder (string item) {
        
        if (item.Contains ("AcidFog")) {
            
            return new List<int> {5, 10, 25 };
        }

        if (item.Contains ("Freezing")) {
            
            return new List<int> {5, 10, 25 };
        }

        if (item.Contains ("Bomb")) {
            
            return new List<int> {5, 10, 25 };
        }

        if (item.Contains ("Gold")) {
            
            return new List<int> {5, 10, 25 };
        }

        if (item.Contains ("Heal")) {
            
            return new List<int> {10, 20, 50 };
        }

        if (item.Contains ("Power")) {
            
            return new List<int> {10, 20, 50 };
        }

        return new List<int> {9, 99, 999 };
    }

    private void SetItemsList () {

        items = Settings.GetItems ();
    }

    private void SetPricesIntreface (string item, float deltaLayer) {

        var counts = GetItemCountPerOrder (item);
        var prices = GetItemPricePerOrder (item);
        
        getCount0.text = counts [0] + "";
        getCount1.text = counts [1] + "";
        getCount2.text = counts [2] + "";
        
        getPrice0.text = prices [0] + "";
        getPrice1.text = prices [1] + "";
        getPrice2.text = prices [2] + "";
        
        name.text = GetItemName (item);
        description.text = GetItemDescription (item);
        
        buy0.OnClick = (b) => {

            if (Settings.diamonds >= prices [0]) {
                
                Settings.AddItemCount (item, counts [0]);
                CreateItemCounts (deltaLayer);

                Settings.diamonds -= prices [0];
            }
        };
        
        buy1.OnClick = (b) => {
            
            if (Settings.diamonds >= prices [1]) {
                
                Settings.AddItemCount (item, counts [1]);
                CreateItemCounts (deltaLayer);

                Settings.diamonds -= prices [1];
            }
        };
        
        buy2.OnClick = (b) => {
            
            if (Settings.diamonds >= prices [2]) {
                
                Settings.AddItemCount (item, counts [2]);
                CreateItemCounts (deltaLayer);

                Settings.diamonds -= prices [2];
            }
        };
    }

    private void CreatePricesInterface (float deltaLayer) {
        
        getCount0 = new GUIText ("0", -2 - deltaLayer, new Vector2 (-8.34f, 3.79f), new Vector2 (0.15f, 0.15f), GUIText.FontName.Font5);
        getCount1 = new GUIText ("0", -2 - deltaLayer, new Vector2 (-8.34f, 2.16f), new Vector2 (0.15f, 0.15f), GUIText.FontName.Font5);
        getCount2 = new GUIText ("0", -2 - deltaLayer, new Vector2 (-8.34f, 0.5f), new Vector2 (0.15f, 0.15f), GUIText.FontName.Font5);
        
        new GUIText ("BUY", -2 - deltaLayer, new Vector2 (-5.55f, 3.79f - 0.1f), new Vector2 (0.09f, 0.09f), GUIText.FontName.Font5);
        new GUIText ("BUY", -2 - deltaLayer, new Vector2 (-5.55f, 2.16f - 0.1f), new Vector2 (0.09f, 0.09f), GUIText.FontName.Font5);
        new GUIText ("BUY", -2 - deltaLayer, new Vector2 (-5.55f, 0.5f - 0.1f), new Vector2 (0.09f, 0.09f), GUIText.FontName.Font5);
        
        getPrice0 = new GUIText ("9999", -2 - deltaLayer, new Vector2 (-3.02f, 3.79f - 0.1f), new Vector2 (0.09f, 0.09f)
            , GUIText.FontName.Font5, false, TextAnchor.MiddleRight);
        getPrice1 = new GUIText ("9999", -2 - deltaLayer, new Vector2 (-3.02f, 2.16f - 0.1f), new Vector2 (0.09f, 0.09f)
            , GUIText.FontName.Font5, false, TextAnchor.MiddleRight);
        getPrice2 = new GUIText ("9999", -2 - deltaLayer, new Vector2 (-3.02f, 0.5f - 0.1f), new Vector2 (0.09f, 0.09f)
            , GUIText.FontName.Font5, false, TextAnchor.MiddleRight);
        
        name = new GUIText ("ACID FOG", -2 - deltaLayer, new Vector2 (5.52f, 3.65f), new Vector2 (0.15f, 0.15f), GUIText.FontName.Font5);
        description = new GUIText ("asdasd\nasdasd", -2 - deltaLayer, new Vector2 (5.52f, 2.99f), new Vector2 (0.13f, 0.13f)
            , GUIText.FontName.Font5, false, TextAnchor.UpperCenter);

        buy0 = new GUIButton ("Textures/Inventory/Buy", -2.1f - deltaLayer, new Vector2 (-4.06f, 3.79f), new Vector2 (-1, -1), false
            , (b) => {

            }, (b) => {

                b += "Pressed";
            }, (b) => {

                b -= "Pressed";
            }
        );
        
        buy1 = new GUIButton ("Textures/Inventory/Buy", -2.1f - deltaLayer, new Vector2 (-4.06f, 2.16f), new Vector2 (-1, -1), false
            , (b) => {

            }, (b) => {

                b += "Pressed";
            }, (b) => {

                b -= "Pressed";
            }
        );
        
        buy2 = new GUIButton ("Textures/Inventory/Buy", -2.1f - deltaLayer, new Vector2 (-4.06f, 0.5f), new Vector2 (-1, -1), false
            , (b) => {

            }, (b) => {

                b += "Pressed";
            }, (b) => {

                b -= "Pressed";
            }
        );
        
        new GUIImage ("Textures/Inventory/Hotspot", -1.9f - deltaLayer, new Vector2 (1.79f, 2.19f), new Vector2 (-1, -1), false);
    }

    private void CreateItemCount (int value, int index, float deltaLayer) {

        itemCounts.Add (new GUIImage ("Textures/SelectLevel/Interface/" + (value / 100) % 10, -2.6f - deltaLayer
            , ItemCountPosition (index) + new Vector2 (-0.5f, 0), new Vector2 (-1, -1), false));
        itemCounts.Add (new GUIImage ("Textures/SelectLevel/Interface/" + (value / 10) % 10, -2.6f - deltaLayer
            , ItemCountPosition (index) + new Vector2 (0f, 0), new Vector2 (-1, -1), false));
        itemCounts.Add (new GUIImage ("Textures/SelectLevel/Interface/" + (value / 1) % 10, -2.6f - deltaLayer
            , ItemCountPosition (index) + new Vector2 (0.5f, 0), new Vector2 (-1, -1), false));
    }

    private void CreateItemCounts (float deltaLayer) {

        if (itemCounts != null) {

            foreach (var i in itemCounts) {

                i.Destroy ();
            }
        }

        itemCounts = new List<GUIImage> ();

        for (int i = currentStart; i < currentStart + 5; i++) {

            CreateItemCount (Settings.GetItemCount (items [i]), i - currentStart, deltaLayer);
        }

    }

    private void CreateItemSelect (int index, float deltaLayer) {

        if (select != null) {

            select.Destroy ();

        }

        selectedIndex = index;

        select = new GUIImage ("Textures/Inventory/GreenSelect", -4.8f - deltaLayer, ItemPosition (index) + new Vector2 (0, 0.87f), new Vector2 (-1, -1), false);
    }

    private Vector2 ItemPosition (int index) {

        return new Vector2 (-7.4f + index * 3.7f, -4.89f);
    }

    private Vector2 ItemCountPosition (int index) {

       
        switch (index) {
            
            case 0:
                return new Vector2 (-7.35f, -1.78f);
            case 1:
                return new Vector2 (-3.69f, -1.78f);
            case 2:
                return new Vector2 (0.00f, -1.78f);
            case 3:
                return new Vector2 (3.69f, -1.78f);
            case 4:
                return new Vector2 (7.35f, -1.78f);
        }

        return new Vector2 (0, 0);
    }

    private void CreateItemButton (string item, int index, float deltaLayer) {

        Vector2 position = ItemPosition (index);

        itemObjects.Add (new GUIButton (GetItemIcon (item), -4.9f - deltaLayer, position + new Vector2 (0, 0), new Vector2 (-1, -1), false
            , (b) => {

                //if (index < currentStart || index >= currentStart + 5) {
                //
                //    return;
                //}

                CreateItemSelect (index, deltaLayer);
                SetPricesIntreface (item, deltaLayer);
            }, (b) => {

            }, (b) => {

            }
        ));
        
        itemObjects.Add (new GUIText (GetItemBottomName (item), -4.8f - deltaLayer, position + new Vector2 (0, -1.49f), new Vector2 (0.09f, 0.09f)
            , GUIText.FontName.Font5));
    }

    private void CreateItems (int start, float deltaLayer) {

        foreach (var a in itemObjects) {

            a.Destroy ();
        }

        itemObjects.Clear ();


        for (int i = start - 1; i < start + 5 + 1; i++) {

            Logger.Log (i, false);

            if ( i >= 0 && i < items.Count) {

                CreateItemButton (items [i], i - start, deltaLayer);
            }
        }

        Logger.Log ();
    }

    private void CreateBottomBarInterface (float deltaLayer) {
        
        new GUIImage ("Textures/Inventory/BottomBackground", -5 - deltaLayer, new Vector2 (0, -4.97f), new Vector2 (-1, -1), false);
        
        new GUIButton ("Textures/Inventory/Left", -2f - deltaLayer, new Vector2 (-10.49f, -4.78f), new Vector2 (-1, -1), false
            , (b) => {

                if (currentStart + 5 < items.Count) {

                    MoveItems (false, () => {
                    
                        currentStart ++;
                        selectedIndex ++;
                        CreateItems (currentStart, deltaLayer);
                        CreateItemCounts (deltaLayer);
                    });
                }
            }, (b) => {

                b += "Pressed";
            }, (b) => {

                b -= "Pressed";
            }
        );
        
        new GUIButton ("Textures/Inventory/Right", -2f - deltaLayer, new Vector2 (10.49f, -4.78f), new Vector2 (-1, -1), false
            , (b) => {
                
                if (currentStart > 0) {

                    MoveItems (true, () => {

                        currentStart --;
                        selectedIndex --;
                        CreateItems (currentStart, deltaLayer);
                        CreateItemCounts (deltaLayer);
                    });
                }
            }, (b) => {

                b += "Pressed";
            }, (b) => {

                b -= "Pressed";
            }
        );
        
        new GUIImage ("Textures/Inventory/CountBar", -2.5f - deltaLayer, ItemCountPosition (0), new Vector2 (-1, -1), false);
        new GUIImage ("Textures/Inventory/CountBar", -2.5f - deltaLayer, ItemCountPosition (1), new Vector2 (-1, -1), false);
        new GUIImage ("Textures/Inventory/CountBar", -2.5f - deltaLayer, ItemCountPosition (2), new Vector2 (-1, -1), false);
        new GUIImage ("Textures/Inventory/CountBar", -2.5f - deltaLayer, ItemCountPosition (3), new Vector2 (-1, -1), false);
        new GUIImage ("Textures/Inventory/CountBar", -2.5f - deltaLayer, ItemCountPosition (4), new Vector2 (-1, -1), false);

        CreateItems (currentStart, deltaLayer);
        CreateItemSelect (currentStart, deltaLayer);
        SetPricesIntreface (items [currentStart], deltaLayer);

        CreateItemCounts (deltaLayer);
    }
    
    private void Create () {

        currentStart = 0;
        var deltaLayer = 0f;
        
        new GUIImage ("Textures/Inventory/Background", -3 - deltaLayer, new Vector2 (0, 0), new Vector2 (-1, -1), false);
        
        var back = new GUIButton ("Textures/Back", -2 - deltaLayer, new Vector2 (11.67f, 5.9f), new Vector2 (-1, -1));
        
        back.OnClick = (b) => {
            
            CameraController.cameraPosition = new Vector2 (0, 0);
            ScenePassageController.instance.LoadScene <SelectLevelController> ((a) => {Destroy (); a(); });
        };
        back.OnButtonDown =  (b) => {

            b += "Pressed";
        };
        back.OnButtonUp = (b) => {

            b -= "Pressed";
        };

        SetItemsList ();

        CreatePricesInterface (deltaLayer);
        CreateBottomBarInterface (deltaLayer);
    }

    private void MoveItems (bool isRight, Actions.VoidVoid onEnd) {

        ForbidController.Forbid ("MovingItems");

        var count = 20;
        var time = 0.5f;
        var distance = 3.7f * 2f;

        UpdateController.LaunchIt (count, time / count, (t) => {

            foreach (var i in itemObjects) {

                i.positionInMeters += new Vector2 ((isRight ? 1 : -1) * distance * (time / count), 0);
            }

            if (select != null) {

                select.positionInMeters += new Vector2 ((isRight ? 1 : -1) * distance * (time / count), 0);
            }
        }, () => {

            ForbidController.Allow ("MovingItems");
            onEnd ();
        });
    }


	public InventoryController () {

        instance = this;

        new ForbidController ();

        UpdateController.toUpdate = Update;
        UpdateController.toFixedUpdate = FixedUpdate;

        new GUIController ();
        
        new SlideController (new Vector2 (0, 0), new Vector2 (0, 0), SlideController.Mode.ReadOnly, 3);
        CameraController.ResizeCamera ( Mathf.Min (CameraController.GetWidthInMeters (1080 / 50f * Settings.FhdToHD), 1920 / 50f * Settings.FhdToHD) );
        CameraController.cameraPosition = new Vector2 (0, 0);

        UpdateController.Timer (0.1f, () => {
            
            itemObjects = new List<GUIObject> ();
            itemCounts = new List<GUIImage> ();

            Create ();
        
        });
    }

	private void FixedUpdate (float deltaTime) {
        
    }

	private void Update (float deltaTime) {
		
		if (Input.GetKeyDown(KeyCode.Escape)) {

            if (!MenusController.RemoveMenus ()) {
                
                CameraController.cameraPosition = new Vector2 (0, 0);
			    ScenePassageController.instance.LoadScene <SelectLevelController> ((a) => {Destroy (); a ();});
            }
        }

        if (!ForbidController.IsForbidden ()) {

            SlideController.frictionDelta = CameraController.widthInMeters/Screen.width;
		    SlideController.instance.Update (deltaTime);
        }
	}

    public void Destroy () {

        UpdateController.StopAllTimers ();
        UpdateController.toFixedUpdate = null;
        UpdateController.toUpdate = null;
        GamePullController.Destroy ();
    }

}
