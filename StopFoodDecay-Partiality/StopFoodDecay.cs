using System;
using Partiality.Modloader;
using UnityEngine;

namespace StopFoodDecay_Partiality
{
    public class MyMod : PartialityMod
    {
        public MyMod()
        {
            this.ModID = "Stop Food Decay";
            this.Version = "1.0.0.0";
            this.author = "Jeremiah";
        }

        public static MyScript script;

        public override void OnEnable()
        {
            base.OnEnable();
            MyScript.mod = this;
            GameObject go = new GameObject();
            script = go.AddComponent<MyScript>();
            script.Initialize();
        }
    }

    public class MyScript : MonoBehaviour
    {
        public static MyMod mod;

        public void Initialize()
        {
            On.Perishable.ItemParentChanged += new On.Perishable.hook_ItemParentChanged(PerishableItemParentChanged_SFDPatch);
        }

        private void PerishableItemParentChanged_SFDPatch(On.Perishable.orig_ItemParentChanged orig, Perishable self)
        {
            if (self.Item != null)
            {
                if (self.Item.IsFood)
                {
                    if (self.DepletionRate != 0f)
                    {
                        self.SetDurabilityDepletion(0f);
                        Debug.Log(String.Format("StopFoodDecay::Perishable.ItemParentChanged: {0}, SetDurabilityDepletion set to {1}.", self.name, 0f));
                    }

                    if (!self.DontPerishInWorld)
                    {
                        self.DontPerishInWorld = true;
                        Debug.Log(String.Format("StopFoodDecay::Perishable.ItemParentChanged: {0}, DontPerishInWorld is now {1}.", self.name, self.DontPerishInWorld));
                    }

                    if (!self.DontPerishSkipTime)
                    {
                        self.DontPerishSkipTime = true;
                        Debug.Log(String.Format("StopFoodDecay::Perishable.ItemParentChanged: {0}, DontPerishSkipTime is now {1}.", self.name, self.DontPerishSkipTime));
                    }
                }
            }

            orig(self);
        }
    }
}