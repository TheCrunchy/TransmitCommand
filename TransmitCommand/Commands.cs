﻿using Sandbox.Game.Entities;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torch.Commands;
using Torch.Commands.Permissions;
using VRage.Game.ModAPI;
using VRageMath;

namespace TransmitCommand
{
    public class Commands : CommandModule
    {
        [Command("transmit", "delete beacons if they arent working")]
        [Permission(MyPromoteLevel.Admin)]
        public void transmit()
        {
            foreach (MyPlayer p in MySession.Static.Players.GetOnlinePlayers())
            {
                List<IMyGps> playergpsList = MyAPIGateway.Session?.GPS.GetGpsList(p.Identity.IdentityId);

                if (playergpsList == null)
                    break;

                foreach (IMyGps gps in playergpsList)
                {

                    if (gps.Description.Contains("Cronch"))
                    {
                        MyAPIGateway.Session?.GPS.RemoveGps(p.Identity.IdentityId, gps);
                    }


                }
            }
            foreach (var group in MyCubeGridGroups.Static.Logical.Groups)
            {
                bool NPC = false;
                foreach (var item in group.Nodes)
                {
                    MyCubeGrid grid = item.NodeData;
                    if (((int)grid.Flags & 4) != 0)
                    {
                        //concealed
                        break;
                    }
                    if (grid.IsStatic)
                    {
                        break;
                    }
                    if (MyGravityProviderSystem.IsPositionInNaturalGravity(grid.PositionComp.GetPosition()))
                    {
                        break;
                    }
                    //this bit requires a fac utils to get the faction tag, you can remove it if you dont need it
                    foreach (long l in grid.BigOwners)
                    {
                        if (FacUtils.GetFactionTag(l) != null && FacUtils.GetFactionTag(l).Length > 3)
                        {
                            NPC = true;

                        }
                    }

                    if (NPC)
                    {
                        break;
                    }
                    int PCU = 0;
                    PCU = grid.BlocksPCU;
                    Vector3 location = grid.PositionComp.GetPosition();
                    //get the gps
                    float broadcastRange = 0;
                    MyGpsCollection gpsCollection = (MyGpsCollection)MyAPIGateway.Session?.GPS;

                    if (PCU <= 10000)
                    {
                        broadcastRange = 3000;

                    }
                    if (PCU < 1000)
                    {
                        broadcastRange = 5;

                    }
                    if (PCU >= 10000)
                    {

                        broadcastRange = 20000;

                    }
                    if (PCU >= 20000)
                    {

                        broadcastRange = 30000;

                    }
                    if (PCU >= 30000)
                    {

                        broadcastRange = 500000;
                    }
                    if (PCU >= 45000)
                    {

                        broadcastRange = 1000000;
                    }
                    if (PCU >= 60000)
                    {

                        broadcastRange = 2000000;
                    }
                    IMyFaction gridOwner = FacUtils.GetPlayersFaction(FacUtils.GetOwner(grid));

                    foreach (MyPlayer p in MySession.Static.Players.GetOnlinePlayers())
                    {


                        List<MyGps> gpsList = new List<MyGps>();
                        float distance = Vector3.Distance(location, p.GetPosition());
                        if (distance <= broadcastRange)
                        {
                            MyGps gps;
                            //add a new gps point if player is in range
                            if (gridOwner != null)
                            {
                                if (gridOwner.IsNeutral(p.Identity.IdentityId) || gridOwner.IsFriendly(p.Identity.IdentityId))
                                {
                                    gps = CreateGps(grid, Color.RoyalBlue, 60, broadcastRange);
                                }
                                else
                                {
                                    gps = CreateGps(grid, Color.DarkRed, 60, broadcastRange);
                                }
                            }
                            else
                            {
                                gps = CreateGps(grid, Color.DarkRed, 60, broadcastRange);
                            }

                            gpsList.Add(gps);

                        }
                        foreach (MyGps gps in gpsList)
                        {

                            MyGps gpsRef = gps;
                            long entityId = 0L;
                            entityId = gps.EntityId;
                            gpsCollection.SendAddGps(p.Identity.IdentityId, ref gpsRef, entityId, false);
                        }

                    }


                }
            }
        }
        private MyGps CreateGps(MyCubeGrid grid, Color gpsColor, long seconds, float distance)
        {

            MyGps gps = new MyGps
            {
                Coords = grid.PositionComp.GetPosition(),
                Name = "Radar Signature - " + distance + " - " + grid.DisplayName,
                DisplayName = "Radar Signature - " + distance + " - " + grid.DisplayName,
                GPSColor = gpsColor,
                IsContainerGPS = true,
                ShowOnHud = true,
                DiscardAt = new TimeSpan?(),
                Description = "Cronch"
            };
            gps.UpdateHash();
            gps.SetEntityId(grid.EntityId);

            return gps;
        }
    }
}
