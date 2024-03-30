﻿using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley.GameData;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Objects;
using StardewValley.Inventories;
using StardewValley.Tools;

namespace AutoEat
{
    /// <summary>The mod entry point.</summary>
    internal sealed class ModEntry : Mod
    {
        bool checkHealth = true;
        bool checkSpeedBuff = true;
        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            helper.Events.Input.ButtonPressed += this.OnButtonPressed;
            helper.Events.GameLoop.UpdateTicked += this.OnUpdateTicked;
            helper.Events.GameLoop.OneSecondUpdateTicked += this.OnOneSecondUpdateTicked;
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Raised after the player presses a button on the keyboard, controller, or mouse.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            // ignore if player hasn't loaded a save yet
            if (!Context.IsWorldReady)
                return;

            // print button presses to the console window
            //this.Monitor.Log($"{Game1.player.Name} pressed {e.Button}.", LogLevel.Debug);
        }

        private void OnUpdateTicked(object sender, EventArgs e)
        {
            if (Game1.player.health > Game1.player.maxHealth * 0.3) checkHealth = true;

            if (Game1.player.health < Game1.player.maxHealth * 0.3 && checkHealth)
            {
                // set variables
                const string Cheese_ID = "424";
                Item cheese = new StardewValley.Object(Cheese_ID, 1, false, -1, 2);
                StardewValley.Object cheeseObj = new StardewValley.Object(Cheese_ID,1,false,-1,2);
                // find cheese 
                int idx = Game1.player.getIndexOfInventoryItem(cheese);

                if(idx >= 0)
                {
                    this.Monitor.Log($"idx:{idx}", LogLevel.Info);
                    checkHealth = false;
                    Game1.player.eatObject(cheeseObj);
                    Game1.player.Items.ReduceId(Cheese_ID, 1);
                }
            } 
        }

        private void OnOneSecondUpdateTicked(object sender, EventArgs e)
        {
            // update checkSpeedBuff
            if (Game1.player.hasBuff("drink"))
            {
                this.Monitor.Log($"Duration: {Game1.player.buffs.AppliedBuffs["drink"].millisecondsDuration}", LogLevel.Warn);
                if (Game1.player.buffs.AppliedBuffs["drink"].millisecondsDuration < 2000)
                {
                    checkSpeedBuff = true;
                }
            }

            // check buff id
            if (!Game1.player.hasBuff("drink") && checkSpeedBuff)
            {
                // set variables TrippleShotEspresso = TSE
                const string TSE_ID = "253";
                Item TSE = new StardewValley.Object(TSE_ID, 1);
                StardewValley.Object TSEObj = new StardewValley.Object(TSE_ID, 1);

                // check buff has speed buff
                if (Game1.player.buffs.Speed == 0 && Game1.player.getIndexOfInventoryItem(TSE) >= 0)
                {
                    checkSpeedBuff = false;
                    Game1.player.eatObject(TSEObj);
                    Game1.player.Items.ReduceId(TSE_ID, 1);
                }
            }

            //this.Monitor.Log($"BuffIDs: {Game1.player.buffs.AppliedBuffIds}", LogLevel.Info);
            //this.Monitor.Log($"BuffApplied: {Game1.player.buffs.Speed}", LogLevel.Info);
            //this.Monitor.Log($"BuffMovement: {Game1.player.getMovementSpeed()}", LogLevel.Info);
        }

    }
}