using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BankMenu {

	public static BankMenu instance;

    private List <GUIObject> objects;
    private List <Vector2> sizes;
    private List <Vector2> positions;
    
    private GUIText moneyText;
    private GUIObject black;
    private float allSize;
    
    private Actions.VoidVoid onEnd;

    private void Scale (float allSizes) {

        for (int i = 0; i < objects.Count; i++) {
            
            objects [i].sizeInMeters = sizes [i] * allSizes;
            objects [i].positionInMeters = positions [i] * allSizes + CameraController.cameraPosition;
        }

    }

    private void CreateLine (Actions.VoidVoid onMoneyChange, Actions.VoidVoid onBuy, Vector2 position, int moneyGot
        , float moneyToSpend, string currency, float deltaLayer, string buttonTexture = "Textures/Bank/Buy", bool isVideo = false) {
        
        objects.Add (new GUIText (moneyGot.ToString (), -0.5f + deltaLayer, position - new Vector2 (-4f, 1.9f) + new Vector2 (-4.09f, 2.8f)
            , new Vector2 (0.16f, 0.16f), GUIText.FontName.Font5, false, TextAnchor.MiddleLeft));
        
        if (!isVideo) {

            var toSpend = new GUIText (moneyToSpend + " " + currency, -0.5f + deltaLayer, position - new Vector2 (-4f, 1.9f) + new Vector2 (-7.25f, 0.94f)
                , new Vector2 (0.11f, 0.11f), GUIText.FontName.Font5, false, TextAnchor.MiddleLeft);

            objects.Add (toSpend);

            objects.Add (new GUIText ("BUY", -0.5f + deltaLayer, position - new Vector2 (-4f, 1.9f) + new Vector2 (-2.44f, 0.94f)
                , new Vector2 (0.11f, 0.11f), GUIText.FontName.Font5));
        }

        var buy = new GUIButton (buttonTexture, -0.6f + deltaLayer, position - new Vector2 (-4f, 1.9f) + new Vector2 (-2.44f, 0.94f), new Vector2 (-1, -1));

        if (isVideo) {

            buy.positionInMeters = new Vector2 (position.x, buy.positionInMeters.y);
        }

        buy.OnClick = (b) => {
            
            onBuy ();
            onMoneyChange ();
        };
        buy.OnButtonDown = (b) => {

            buy += "Pressed";
        };
        buy.OnButtonUp = (b) => {
            
            buy -= "Pressed";
        };
        
        objects.Add (buy);
    }

    public BankMenu (Actions.VoidVoid onMoneyChange = null, Actions.VoidVoid _onEnd = null, float deltaLayer = 0) {
        
        new SlideController (0, 0, SlideController.Mode.ReadOnly, 3);

        objects = new List<GUIObject> ();
        sizes = new List<Vector2> ();
        positions = new List<Vector2> ();

        onEnd = _onEnd;

        instance = this;
        
        objects.Add (new GUIImage ("Textures/SelectLevel/Interface/" + (Settings.diamonds / 1000) % 10, -0.5f + deltaLayer
            , new Vector2 (-0.1f, 5.54f), new Vector2 (-1, -1)));
        objects.Add (new GUIImage ("Textures/SelectLevel/Interface/" + (Settings.diamonds / 100) % 10, -0.5f + deltaLayer
            , new Vector2 (0.4f, 5.54f), new Vector2 (-1, -1)));
        objects.Add (new GUIImage ("Textures/SelectLevel/Interface/" + (Settings.diamonds / 10) % 10, -0.5f + deltaLayer
            , new Vector2 (0.9f, 5.54f), new Vector2 (-1, -1)));
        objects.Add (new GUIImage ("Textures/SelectLevel/Interface/" + (Settings.diamonds / 1) % 10, -0.5f + deltaLayer
            , new Vector2 (1.39f, 5.54f), new Vector2 (-1, -1)));

        objects.Add (new GUIImage ("Textures/SelectLevel/Interface/DiamondsBar", -0.4f + deltaLayer, new Vector2 (0.64f, 5.54f), new Vector2 (-1, -1)));
         

        black = new GUIImageAlpha ("Textures/Black", -1.2f + deltaLayer, new Vector2 (0, 0) + CameraController.cameraPosition
            , new Vector2 (CameraController.widthInMeters, CameraController.heightInMeters));
        black.color = new Color (1, 1, 1, 0);
        black.isClickable = true;
        
        objects.Add (new GUIImage ("Textures/Bank/Background", -1.1f + deltaLayer, new Vector2 (0, 0), new Vector2 (-1, -1)));
        
        var back = new GUIButton ("Textures/Back", -1.0f + deltaLayer, new Vector2 (7.31f, 5.58f), new Vector2 (-1, -1));
        back.OnButtonDown = (b) => {
            
            back += "Pressed";
        };
        back.OnButtonUp = (b) => {
            
            back -= "Pressed";
        };
        back.OnClick = (b) => {
            
            Destroy ();
        };

        objects.Add (back);
        
        
        //moneyText = new GUIText (Settings.money.ToString (), -0.5f + deltaLayer, new Vector2 (0.5f, 13f)
        //    , new Vector2 (0.3f, 0.3f), GUIText.FontName.Font5);

        //objects.Add (moneyText);
        
        CreateLine (onMoneyChange, onMoneyChange, new Vector2 (-4f, 1.9f), 30, 0.99f, "USD", deltaLayer, "Textures/Bank/Video", true);
        CreateLine (onMoneyChange, onMoneyChange, new Vector2 (4f, 1.9f), 100, 2.99f, "USD", deltaLayer);
        CreateLine (onMoneyChange, onMoneyChange, new Vector2 (-4f, -2.99f), 200, 4.99f, "USD", deltaLayer);
        CreateLine (onMoneyChange, onMoneyChange, new Vector2 (4f, -2.99f), 500, 9.99f, "USD", deltaLayer);
        //CreateLine (onMoneyChange, 1, 150, 1.5f, "USD", deltaLayer);
        //CreateLine (onMoneyChange, 2, 300, 3, "USD", deltaLayer);
        //CreateLine (onMoneyChange, 3, 750, 4.5f, "USD", deltaLayer);
        
        foreach (var w in objects) {

            sizes.Add (w.sizeInMeters);
            positions.Add (w.positionInMeters);
        }

        allSize = 0;
        bool isScaled = false;

        Scale (0);

        UpdateController.AddFixedUpdatable ("BankMenu", (f) => {

            allSize += f * 2f;

            if (!isScaled) {

                if (allSize > 1) {

                    allSize = 1f;
                    isScaled = true;
                }

                Scale (allSize);
            }

            if (!isScaled) {

                black.color = new Color (1, 1, 1, allSize * 0.84f);
            }

            black.positionInMeters = CameraController.cameraPosition;
        });
    }

    public void Destroy () {

        allSize = Mathf.Min (allSize, 1);
        Scale (allSize);
        instance = null;

        if (onEnd != null) {

            onEnd ();
        }
        
        UpdateController.RemoveFixedUpdatable ("BankMenu");
        UpdateController.AddFixedUpdatable ("BankMenuDestroying", (f) => {

            allSize -= f * 2f;
            
            if (allSize <= 0) {

                    
                foreach (var w in objects) {

                    w.Destroy ();
                }

                black.Destroy ();
        
                UpdateController.RemoveFixedUpdatable ("BankMenuDestroying");
                return;
            }

            Scale (allSize);

            black.color = new Color (1, 1, 1, allSize * 0.84f); 
            black.positionInMeters = CameraController.cameraPosition;
        });
    }
}
