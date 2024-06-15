using Dalamud.Game.ClientState.JobGauge.Types;
using XIVSlothComboX.Combos.PvE.Content;
using XIVSlothComboX.Core;
using XIVSlothComboX.CustomComboNS;
using System;
using FFXIVClientStructs.FFXIV.Client.Game;
using XIVSlothComboX.Extensions;
using XIVSlothComboX.Services;

namespace XIVSlothComboX.Combos.PvE
{
    internal static class SMN
    {
        public const byte ClassID = 26;
        public const byte JobID = 27;

        public const float CooldownThreshold = 0.5f;

        public const uint
            // Summons
            // SummonRuby = 25802,
            // SummonTopaz = 25803,
            // SummonEmerald = 25804,
            SummonRuby_红宝石召唤 = 25802,
            SummonTopaz_黄宝石召唤 = 25803,
            SummonEmerald_绿宝石召唤 = 25804,

            // SummonIfrit = 25805,
            // SummonTitan = 25806,
            // SummonGaruda = 25807,

            // SummonIfrit2 = 25838,
            // SummonTitan2 = 25839,
            // SummonGaruda2 = 25840,
            SummonIfrit = 25805,
            SummonTitan = 25806,
            SummonGaruda = 25807,
            SummonIfrit2 = 25838,
            SummonTitan2 = 25839,
            SummonGaruda2 = 25840,
            SummonCarbuncle = 25798,

            // Summon abilities
            宝石耀Gemshine = 25883,
            宝石辉AoePreciousBrilliance = 25884,
            DreadwyrmTrance = 3581,

            // Summon Ruins
            RubyRuin1 = 25808,
            RubyRuin2 = 25811,
            RubyRuin3 = 25817,
            TopazRuin1 = 25809,
            TopazRuin2 = 25812,
            TopazRuin3 = 25818,
            EmeralRuin1 = 25810,
            EmeralRuin2 = 25813,
            EmeralRuin3 = 25819,

            // Summon Outbursts
            Outburst = 16511,
            RubyOutburst = 25814,
            TopazOutburst = 25815,
            EmeraldOutburst = 25816,

            // Summon single targets
            RubyRite = 25823,
            TopazRite = 25824,
            EmeraldRite = 25825,

            // Summon AoEs
            RubyCata = 25832,
            TopazCata = 25833,
            EmeraldCata = 25834,

            // Summon Astral Flows
            CrimsonCyclone = 25835, // Dash
            CrimsonStrike = 25885, // Melee
            MountainBuster = 25836,
            螺旋气流Slipstream = 25837,

            // Demi summons
            龙神召唤SummonBahamut = 7427,
            不死鸟召唤SummonPhoenix = 25831,

            // Demi summon abilities
            星极脉冲AstralImpulse = 25820, // Single target Bahamut GCD
            星极核爆AstralFlare = 25821, // AoE Bahamut GCD
            //死星核爆 Deathflare
            死星核爆 = 3582, // Damage oGCD Bahamut
            //龙神迸发 EnkindleBahamut
            龙神迸发 = 7429,
            灵泉之炎FountainOfFire = 16514, // Single target Phoenix GCD
            炼狱之炎BrandOfPurgatory = 16515, // AoE Phoenix GCD
            Rekindle = 25830, // Healing oGCD Phoenix
            EnkindlePhoenix = 16516,

            // Shared summon abilities  星极超流 AstralFlow
            星极超流 = 25822,

            // Summoner GCDs
            Ruin = 163,
            Ruin2 = 172,
            Ruin3 = 3579,
            毁绝Ruin4 = 7426,
            Tridisaster = 25826,

            // Summoner AoE
            RubyDisaster = 25827,
            TopazDisaster = 25828,
            EmeraldDisaster = 25829,

            // Summoner oGCDs
            能量吸收EnergyDrain = 16508,
            溃烂爆发Fester = 181,
            能量抽取EnergySiphon = 16510,
            痛苦核爆Painflare = 3578,

            // Revive
            Resurrection = 173,

            // Buff 
            RadiantAegis = 25799,
            以太蓄能Aethercharge = 25800,
            //灼热之光 SearingLight
            灼热之光SearingLight = 25801;


        public static class Buffs
        {
            public const ushort
                FurtherRuin = 2701,
                螺旋气流GarudasFavor = 2725,
                TitansFavor = 2853,
                IfritsFavor = 2724,
                EverlastingFlight = 16517,
                SearingLight = 2703;
        }

        public static class Config
        {
            public const string
                SMN_Lucid = "SMN_Lucid",
                SMN_BurstPhase = "SMN_BurstPhase",
                召唤顺序 = "SMN_PrimalChoice",
                // SMN_PrimalChoice = "SMN_PrimalChoice",
                SMN_SwiftcastPhase = "SMN_SwiftcastPhase",
                SMN_Burst_Delay = "SMN_Burst_Delay",
                SMN_VariantCure = "SMN_VariantCure";
        }

        internal class SMN_Raise : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SMN_Raise;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID == All.即刻咏唱Swiftcast)
                {
                    if (HasEffect(All.Buffs.Swiftcast) && IsEnabled(CustomComboPreset.SMN_Variant_Raise) && IsEnabled(Variant.VariantRaise))
                        return Variant.VariantRaise;

                    if (IsOnCooldown(All.即刻咏唱Swiftcast))
                        return Resurrection;
                }

                return actionID;
            }
        }

        internal class SMN_RuinMobility : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SMN_RuinMobility;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID == 毁绝Ruin4)
                {
                    var furtherRuin = HasEffect(Buffs.FurtherRuin);

                    if (!furtherRuin)
                        return Ruin3;
                }

                return actionID;
            }
        }

        internal class SMN_EDFester : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SMN_EDFester;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID == 溃烂爆发Fester)
                {
                    var gauge = GetJobGauge<SMNGauge>();
                    // if (HasEffect(Buffs.FurtherRuin) && IsOnCooldown(EnergyDrain) && !gauge.HasAetherflowStacks && IsEnabled(CustomComboPreset.SMN_EDFester_Ruin4))
                    //     return Ruin4;

                    if (LevelChecked(能量吸收EnergyDrain) && !gauge.HasAetherflowStacks)
                        return 能量吸收EnergyDrain;
                }

                return actionID;
            }
        }

        internal class SMN_ESPainflare : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SMN_ESPainflare;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                var gauge = GetJobGauge<SMNGauge>();

                if (actionID == 痛苦核爆Painflare && LevelChecked(痛苦核爆Painflare) && !gauge.HasAetherflowStacks)
                {
                    if (HasEffect(Buffs.FurtherRuin) && IsOnCooldown(能量抽取EnergySiphon) && IsEnabled(CustomComboPreset.SMN_ESPainflare_Ruin4))
                        return 毁绝Ruin4;

                    if (LevelChecked(能量抽取EnergySiphon))
                        return 能量抽取EnergySiphon;

                    if (LevelChecked(能量吸收EnergyDrain))
                        return 能量吸收EnergyDrain;
                }

                return actionID;
            }
        }

        /**
         * 高级召唤
         */
        internal class SMN_Advanced_Combo : CustomCombo
        {
            internal static uint DemiAttackCount = 0;
            internal static bool UsedDemiAttack = false;
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SMN_Advanced_Combo;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                var gauge = GetJobGauge<SMNGauge>();
                // var summonerPrimalChoice = PluginConfiguration.GetCustomIntValue(Config.召唤顺序);
                var 召唤顺序 = PluginConfiguration.GetCustomIntValue(Config.召唤顺序);
                var 在什么阶段用爆发 = PluginConfiguration.GetCustomIntValue(Config.SMN_BurstPhase);
                var lucidThreshold = PluginConfiguration.GetCustomIntValue(Config.SMN_Lucid);
                var 即可咏唱swiftcastPhase = PluginConfiguration.GetCustomIntValue(Config.SMN_SwiftcastPhase);
                //0-3
                var 延迟几个GCD打爆发 = PluginConfiguration.GetCustomIntValue(Config.SMN_Burst_Delay);
                var inOpener = CombatEngageDuration().TotalSeconds < 40;
                var STCombo = actionID is Ruin or Ruin2;
                var AoECombo = actionID is Outburst or Tridisaster;

                if (WasLastAction(OriginalHook(以太蓄能Aethercharge)))
                {
                    DemiAttackCount = 0; // Resets counter
                }


                // If SMN_Advanced_Burst_Delay_Option is active and outside opener window, set DemiAttackCount to 6 to ignore delayed oGCDs 
                if (!inOpener)
                // if (IsEnabled(CustomComboPreset.SMN_Advanced_Burst_Delay_Option) && !inOpener)
                {
                    DemiAttackCount = 6;
                }

                // Sets DemiAttackCount to 6 if for whatever reason you're in a position that you can't demi attack to prevent ogcd waste.
                if (GetCooldown(OriginalHook(以太蓄能Aethercharge)).CooldownElapsed >= 12.5)
                {
                    DemiAttackCount = 6;
                }


                if (gauge.SummonTimerRemaining == 0 && !InCombat())
                {
                    DemiAttackCount = 0;
                }


                //CHECK_DEMIATTACK_USE
                if (UsedDemiAttack == false && lastComboMove is 星极脉冲AstralImpulse or 灵泉之炎FountainOfFire or 星极核爆AstralFlare or 炼狱之炎BrandOfPurgatory &&
                    DemiAttackCount is not 6 && GetCooldownRemainingTime(星极脉冲AstralImpulse) > 1)
                {
                    UsedDemiAttack = true; // Registers that a Demi Attack was used and blocks further incrementation of DemiAttackCountCount
                    DemiAttackCount++; // Increments DemiAttack counter
                }

                //CHECK_DEMIATTACK_USE_RESET
                if (UsedDemiAttack && GetCooldownRemainingTime(星极脉冲AstralImpulse) < 1)
                {
                    // Resets block to allow CHECK_DEMIATTACK_USE
                    UsedDemiAttack = false;
                }


                if (actionID is Ruin or Ruin2 or Outburst or Tridisaster)
                {
                    if (IsEnabled(CustomComboPreset.SMN_Variant_Cure) && IsEnabled(Variant.VariantCure) &&
                        PlayerHealthPercentageHp() <= GetOptionValue(Config.SMN_VariantCure))
                        return Variant.VariantCure;

                    if (IsEnabled(CustomComboPreset.SMN_Variant_Rampart) &&
                        IsEnabled(Variant.VariantRampart) &&
                        IsOffCooldown(Variant.VariantRampart) &&
                        CanSpellWeave(actionID))
                        return Variant.VariantRampart;


                    if (CanSpellWeavePlus(actionID))
                    {
                        // Searing Light
                        if (IsEnabled(CustomComboPreset.SMN_SearingLight) && 灼热之光SearingLight.ActionReady())
                        {
                            if ((在什么阶段用爆发 is 0 or 1 && OriginalHook(Ruin) == 星极脉冲AstralImpulse) ||
                                (在什么阶段用爆发 == 2 && OriginalHook(Ruin) == 灵泉之炎FountainOfFire) ||
                                (在什么阶段用爆发 == 3 && OriginalHook(Ruin) is 星极脉冲AstralImpulse or 灵泉之炎FountainOfFire) ||
                                (在什么阶段用爆发 == 4))
                            {
                                if (STCombo || (AoECombo && IsNotEnabled(CustomComboPreset.SMN_SearingLight_STOnly)))
                                    return 灼热之光SearingLight;
                            }

                            else return 灼热之光SearingLight;
                        }
                    }

                    // if (CanSpellWeave(actionID))
                    if (CanSpellWeavePlus(actionID))
                    {
                        // Emergency priority Demi Nuke to prevent waste if you can't get demi attacks out to satisfy the slider check.
                        if (OriginalHook(Ruin) is 星极脉冲AstralImpulse or 灵泉之炎FountainOfFire &&
                            GetCooldown(OriginalHook(以太蓄能Aethercharge)).CooldownElapsed >= 12.5)
                        {
                            if (IsEnabled(CustomComboPreset.SMN_Advanced_Combo_DemiSummons_Attacks))
                            {
                                if (IsOffCooldown(OriginalHook(龙神迸发)) && GetCooldownRemainingTime(灼热之光SearingLight) is >= 10 &&
                                    LevelChecked(龙神召唤SummonBahamut))
                                    return OriginalHook(龙神迸发);

                                if (IsOffCooldown(死星核爆) && GetCooldownRemainingTime(灼热之光SearingLight) is >= 10 && LevelChecked(死星核爆) &&
                                    OriginalHook(Ruin) is 星极脉冲AstralImpulse)
                                    return OriginalHook(星极超流);
                            }

                            // Demi Nuke 2: Electric Boogaloo
                            if (IsEnabled(CustomComboPreset.SMN_Advanced_Combo_DemiSummons_Rekindle))
                            {
                                if (IsOffCooldown(Rekindle) && OriginalHook(Ruin) is 灵泉之炎FountainOfFire)
                                    return OriginalHook(星极超流);
                            }
                        }

                        // ED/ES
                        if (IsEnabled(CustomComboPreset.SMN_Advanced_Combo_EDFester) && !gauge.HasAetherflowStacks &&
                            IsOffCooldown(能量吸收EnergyDrain) &&
                            (!LevelChecked(DreadwyrmTrance) || (!inOpener || DemiAttackCount >= 延迟几个GCD打爆发)))
                        {
                            if ((STCombo || (AoECombo && !LevelChecked(能量抽取EnergySiphon))) && LevelChecked(能量吸收EnergyDrain))
                                return 能量吸收EnergyDrain;

                            if (AoECombo && LevelChecked(能量抽取EnergySiphon))
                                return 能量抽取EnergySiphon;
                        }

                        // First set Fester/Painflare if ED is close to being off CD, or off CD while you have aetherflow stacks.
                        if (IsEnabled(CustomComboPreset.SMN_Advanced_Combo_EDFester) && IsEnabled(CustomComboPreset.SMN_DemiEgiMenu_oGCDPooling) &&
                            gauge.HasAetherflowStacks)
                        {
                            if (GetCooldown(能量吸收EnergyDrain).CooldownRemaining <= 3.2)
                            {
                                if ((HasEffect(Buffs.SearingLight) && IsNotEnabled(CustomComboPreset.SMN_Advanced_Burst_Any_Option) ||
                                     HasEffectAny(Buffs.SearingLight)) &&
                                    (在什么阶段用爆发 is not 4) ||
                                    (在什么阶段用爆发 == 4 && !HasEffect(Buffs.TitansFavor)))
                                {
                                    if (STCombo)
                                        return 溃烂爆发Fester;

                                    if (AoECombo && LevelChecked(痛苦核爆Painflare) && IsNotEnabled(CustomComboPreset.SMN_DemiEgiMenu_oGCDPooling_Only))
                                        return 痛苦核爆Painflare;
                                }
                            }
                        }

                        // Demi Nuke
                        if (OriginalHook(Ruin) is 星极脉冲AstralImpulse or 灵泉之炎FountainOfFire)
                        {
                            if (IsEnabled(CustomComboPreset.SMN_Advanced_Combo_DemiSummons_Attacks) && DemiAttackCount >= 延迟几个GCD打爆发)
                            {
                                if (IsOffCooldown(OriginalHook(龙神迸发)) && GetCooldownRemainingTime(灼热之光SearingLight) is >= 10 &&
                                    LevelChecked(龙神召唤SummonBahamut))
                                    return OriginalHook(龙神迸发);

                                if (IsOffCooldown(死星核爆) && LevelChecked(死星核爆) && GetCooldownRemainingTime(灼热之光SearingLight) is >= 10 &&
                                    OriginalHook(Ruin) is 星极脉冲AstralImpulse)
                                    return OriginalHook(星极超流);
                            }

                            // Demi Nuke 2: Electric Boogaloo
                            if (IsEnabled(CustomComboPreset.SMN_Advanced_Combo_DemiSummons_Rekindle))
                            {
                                if (IsOffCooldown(Rekindle) && OriginalHook(Ruin) is 灵泉之炎FountainOfFire)
                                    return OriginalHook(星极超流);
                            }
                        }

                        // Fester/Painflare
                        if (IsEnabled(CustomComboPreset.SMN_Advanced_Combo_EDFester))
                        {
                            if (gauge.HasAetherflowStacks && CanSpellWeave(actionID))
                            {
                                if (IsNotEnabled(CustomComboPreset.SMN_DemiEgiMenu_oGCDPooling))
                                {
                                    if (STCombo)
                                        return 溃烂爆发Fester;

                                    if (AoECombo && LevelChecked(痛苦核爆Painflare))
                                        return 痛苦核爆Painflare;
                                }

                                if (IsEnabled(CustomComboPreset.SMN_DemiEgiMenu_oGCDPooling))
                                {
                                    if (!LevelChecked(灼热之光SearingLight))
                                    {
                                        if (STCombo)
                                            return 溃烂爆发Fester;

                                        if (AoECombo && LevelChecked(痛苦核爆Painflare) &&
                                            IsNotEnabled(CustomComboPreset.SMN_DemiEgiMenu_oGCDPooling_Only))
                                            return 痛苦核爆Painflare;
                                    }

                                    if ((HasEffect(Buffs.SearingLight) && IsNotEnabled(CustomComboPreset.SMN_Advanced_Burst_Any_Option) ||
                                         HasEffectAny(Buffs.SearingLight)) &&
                                        (在什么阶段用爆发 is 0 or 1 or 2 or 3 && DemiAttackCount >= 延迟几个GCD打爆发) ||
                                        (在什么阶段用爆发 == 4 && !HasEffect(Buffs.TitansFavor)))
                                    {
                                        if (STCombo)
                                            return 溃烂爆发Fester;

                                        if (AoECombo && LevelChecked(痛苦核爆Painflare) &&
                                            IsNotEnabled(CustomComboPreset.SMN_DemiEgiMenu_oGCDPooling_Only))
                                            return 痛苦核爆Painflare;
                                    }
                                }
                            }
                        }

                        // Lucid Dreaming
                        if (IsEnabled(CustomComboPreset.SMN_Lucid) && ActionReady(All.LucidDreaming) && LocalPlayer.CurrentMp <= lucidThreshold)
                            return All.LucidDreaming;
                    }

                    // Demi
                    if (IsEnabled(CustomComboPreset.SMN_Advanced_Combo_DemiSummons))
                    {
                        if (InCombat() && gauge.SummonTimerRemaining == 0 && IsOffCooldown(OriginalHook(以太蓄能Aethercharge)) &&
                            (LevelChecked(以太蓄能Aethercharge) && !LevelChecked(龙神召唤SummonBahamut) || // Pre-Bahamut Phase
                             gauge.IsBahamutReady && LevelChecked(龙神召唤SummonBahamut) || // Bahamut Phase
                             gauge.IsPhoenixReady && LevelChecked(不死鸟召唤SummonPhoenix))) // Phoenix Phase
                            return OriginalHook(以太蓄能Aethercharge);
                    }

                    //Ruin4 in Egi Phases
                    if (IsEnabled(CustomComboPreset.SMN_Advanced_Combo_Ruin4) && HasEffect(Buffs.FurtherRuin) &&
                        ((!HasEffect(All.Buffs.Swiftcast) && IsMoving && ((HasEffect(Buffs.螺旋气流GarudasFavor) && !gauge.IsGarudaAttuned) ||
                                                                          (gauge.IsIfritAttuned && lastComboMove is not CrimsonCyclone))) ||
                         GetCooldownRemainingTime(OriginalHook(以太蓄能Aethercharge)) is < 2.5f and > 0))
                        return 毁绝Ruin4;

                    // Egi Features
                    if (IsEnabled(CustomComboPreset.SMN_DemiEgiMenu_SwiftcastEgi) && LevelChecked(All.即刻咏唱Swiftcast))
                    {
                        // Swiftcast Garuda Feature
                        if (即可咏唱swiftcastPhase is 0 or 1 && LevelChecked(螺旋气流Slipstream) && HasEffect(Buffs.螺旋气流GarudasFavor))
                        {
                            if (CanSpellWeave(actionID) && gauge.IsGarudaAttuned && IsOffCooldown(All.即刻咏唱Swiftcast))
                            {
                                if (STCombo || (AoECombo && IsNotEnabled(CustomComboPreset.SMN_DemiEgiMenu_SwiftcastEgi_Only)))
                                    return All.即刻咏唱Swiftcast;
                            }

                            if (IsEnabled(CustomComboPreset.SMN_Garuda_Slipstream) &&
                                ((HasEffect(Buffs.螺旋气流GarudasFavor) && HasEffect(All.Buffs.Swiftcast)) ||
                                 (gauge.Attunement == 0))) // Astral Flow if Swiftcast is not ready throughout Garuda
                                return OriginalHook(星极超流);
                        }

                        // Swiftcast Ifrit Feature (Conditions to allow for SpS Ruins to still be under the effect of Swiftcast)
                        if (即可咏唱swiftcastPhase == 2)
                        {
                            if (IsOffCooldown(All.即刻咏唱Swiftcast) && gauge.IsIfritAttuned && lastComboMove is not CrimsonCyclone)
                            {
                                if (IsNotEnabled(CustomComboPreset.SMN_Ifrit_Cyclone) ||
                                    (IsEnabled(CustomComboPreset.SMN_Ifrit_Cyclone) && gauge.Attunement >= 1))
                                {
                                    if (STCombo || (AoECombo && IsNotEnabled(CustomComboPreset.SMN_DemiEgiMenu_SwiftcastEgi_Only)))
                                        return All.即刻咏唱Swiftcast;
                                }
                            }
                        }

                        // SpS Swiftcast
                        if (即可咏唱swiftcastPhase == 3)
                        {
                            // Swiftcast Garuda Feature
                            if (LevelChecked(螺旋气流Slipstream) && HasEffect(Buffs.螺旋气流GarudasFavor))
                            {
                                if (CanSpellWeave(actionID) && gauge.IsGarudaAttuned && IsOffCooldown(All.即刻咏唱Swiftcast))
                                {
                                    if (STCombo || (AoECombo && IsNotEnabled(CustomComboPreset.SMN_DemiEgiMenu_SwiftcastEgi_Only)))
                                        return All.即刻咏唱Swiftcast;
                                }

                                if (IsEnabled(CustomComboPreset.SMN_Garuda_Slipstream) &&
                                    ((HasEffect(Buffs.螺旋气流GarudasFavor) && HasEffect(All.Buffs.Swiftcast)) ||
                                     (gauge.Attunement == 0))) // Astral Flow if Swiftcast is not ready throughout Garuda
                                    return OriginalHook(星极超流);
                            }

                            // Swiftcast Ifrit Feature (Conditions to allow for SpS Ruins to still be under the effect of Swiftcast)
                            if (IsOffCooldown(All.即刻咏唱Swiftcast) && gauge.IsIfritAttuned && lastComboMove is not CrimsonCyclone)
                            {
                                if (IsNotEnabled(CustomComboPreset.SMN_Ifrit_Cyclone) ||
                                    (IsEnabled(CustomComboPreset.SMN_Ifrit_Cyclone) && gauge.Attunement >= 1))
                                {
                                    if (STCombo || (AoECombo && IsNotEnabled(CustomComboPreset.SMN_DemiEgiMenu_SwiftcastEgi_Only)))
                                        return All.即刻咏唱Swiftcast;
                                }
                            }
                        }
                    }

                    // Gemshine/Precious Brilliance priority casting
                    if (IsEnabled(CustomComboPreset.SMN_Advanced_Combo_EgiSummons_Attacks) &&
                        ((gauge.IsIfritAttuned && gauge.Attunement >= 1 && HasEffect(All.Buffs.Swiftcast) && lastComboMove is not CrimsonCyclone) ||
                         (HasEffect(Buffs.螺旋气流GarudasFavor) && gauge.Attunement >= 1 && !HasEffect(All.Buffs.Swiftcast) && IsMoving)))
                    {
                        if (STCombo)
                            return OriginalHook(宝石耀Gemshine);

                        if (AoECombo && LevelChecked(宝石辉AoePreciousBrilliance))
                            return OriginalHook(宝石辉AoePreciousBrilliance);
                    }

                    if (IsEnabled(CustomComboPreset.SMN_Garuda_Slipstream) && HasEffect(Buffs.螺旋气流GarudasFavor) &&
                        (IsNotEnabled(CustomComboPreset.SMN_DemiEgiMenu_SwiftcastEgi) || 即可咏唱swiftcastPhase == 2) || // Garuda
                        IsEnabled(CustomComboPreset.SMN_Titan_MountainBuster) && HasEffect(Buffs.TitansFavor) &&
                        lastComboMove is TopazRite or TopazCata && CanSpellWeavePlus(actionID) || // Titan
                        IsEnabled(CustomComboPreset.SMN_Ifrit_Cyclone) &&
                        ((HasEffect(Buffs.IfritsFavor) &&
                          (IsNotEnabled(CustomComboPreset.SMN_Ifrit_Cyclone_Option) || (IsMoving || gauge.Attunement == 0))) ||
                         (lastComboMove == CrimsonCyclone && InMeleeRange()))) // Ifrit
                    {
                        return OriginalHook(星极超流);
                    }


                    // Gemshine/Precious Brilliance
                    if (IsEnabled(CustomComboPreset.SMN_Advanced_Combo_EgiSummons_Attacks) &&
                        (gauge.IsGarudaAttuned || gauge.IsTitanAttuned || gauge.IsIfritAttuned))
                    {
                        if (STCombo)
                            return OriginalHook(宝石耀Gemshine);

                        if (AoECombo && LevelChecked(宝石辉AoePreciousBrilliance))
                            return OriginalHook(宝石辉AoePreciousBrilliance);
                    }

                    // Egi Order
                    if (IsEnabled(CustomComboPreset.SMN_DemiEgiMenu_EgiOrder) && gauge.SummonTimerRemaining == 0 &&
                        IsOnCooldown(不死鸟召唤SummonPhoenix) &&
                        IsOnCooldown(龙神召唤SummonBahamut))
                    {
                        switch (召唤顺序)
                        {
                            case 1:
                            {
                                if (gauge.IsTitanReady && LevelChecked(SummonTopaz_黄宝石召唤))
                                    return OriginalHook(SummonTopaz_黄宝石召唤);

                                if (gauge.IsGarudaReady && LevelChecked(SummonEmerald_绿宝石召唤))
                                    return OriginalHook(SummonEmerald_绿宝石召唤);

                                if (gauge.IsIfritReady && LevelChecked(SummonRuby_红宝石召唤))
                                    return OriginalHook(SummonRuby_红宝石召唤);

                                break;
                            }

                            case 2:
                            {
                                if (gauge.IsTitanReady && LevelChecked(SummonTopaz_黄宝石召唤))
                                    return OriginalHook(SummonTopaz_黄宝石召唤);

                                if (gauge.IsIfritReady && LevelChecked(SummonRuby_红宝石召唤))
                                    return OriginalHook(SummonRuby_红宝石召唤);

                                if (gauge.IsGarudaReady && LevelChecked(SummonEmerald_绿宝石召唤))
                                    return OriginalHook(SummonEmerald_绿宝石召唤);

                                break;
                            }

                            case 3:
                            {
                                if (gauge.IsGarudaReady && LevelChecked(SummonEmerald_绿宝石召唤))
                                    return OriginalHook(SummonEmerald_绿宝石召唤);

                                if (gauge.IsTitanReady && LevelChecked(SummonTopaz_黄宝石召唤))
                                    return OriginalHook(SummonTopaz_黄宝石召唤);

                                if (gauge.IsIfritReady && LevelChecked(SummonRuby_红宝石召唤))
                                    return OriginalHook(SummonRuby_红宝石召唤);


                                break;
                            }
                            case 4:
                            {
                                if (gauge.IsGarudaReady && LevelChecked(SummonEmerald_绿宝石召唤))
                                    return OriginalHook(SummonEmerald_绿宝石召唤);

                                if (gauge.IsIfritReady && LevelChecked(SummonRuby_红宝石召唤))
                                    return OriginalHook(SummonRuby_红宝石召唤);

                                if (gauge.IsTitanReady && LevelChecked(SummonTopaz_黄宝石召唤))
                                    return OriginalHook(SummonTopaz_黄宝石召唤);
                                break;
                            }

                            case 5:
                            {
                                if (gauge.IsIfritReady && LevelChecked(SummonRuby_红宝石召唤))
                                    return OriginalHook(SummonRuby_红宝石召唤);

                                if (gauge.IsGarudaReady && LevelChecked(SummonEmerald_绿宝石召唤))
                                    return OriginalHook(SummonEmerald_绿宝石召唤);

                                if (gauge.IsTitanReady && LevelChecked(SummonTopaz_黄宝石召唤))
                                    return OriginalHook(SummonTopaz_黄宝石召唤);

                                break;
                            }
                            case 6:
                            {
                                if (gauge.IsIfritReady && LevelChecked(SummonRuby_红宝石召唤))
                                    return OriginalHook(SummonRuby_红宝石召唤);

                                if (gauge.IsTitanReady && LevelChecked(SummonTopaz_黄宝石召唤))
                                    return OriginalHook(SummonTopaz_黄宝石召唤);

                                if (gauge.IsGarudaReady && LevelChecked(SummonEmerald_绿宝石召唤))
                                    return OriginalHook(SummonEmerald_绿宝石召唤);
                                break;
                            }

                            default:
                            {
                                if (gauge.IsTitanReady && LevelChecked(SummonTopaz_黄宝石召唤))
                                    return OriginalHook(SummonTopaz_黄宝石召唤);

                                if (gauge.IsGarudaReady && LevelChecked(SummonEmerald_绿宝石召唤))
                                    return OriginalHook(SummonEmerald_绿宝石召唤);

                                if (gauge.IsIfritReady && LevelChecked(SummonRuby_红宝石召唤))
                                    return OriginalHook(SummonRuby_红宝石召唤);
                                break;
                            }
                        }
                    }

                    // Ruin 4
                    // if (IsEnabled(CustomComboPreset.SMN_Advanced_Combo_Ruin4) && LevelChecked(毁绝Ruin4) && gauge.SummonTimerRemaining == 0 &&
                    //     gauge.AttunmentTimerRemaining == 0 && HasEffect(Buffs.FurtherRuin))
                    //     return 毁绝Ruin4;
                }

                return actionID;
            }
        }

        internal class SMN_CarbuncleReminder : CustomCombo
        {
            protected internal override CustomComboPreset Preset => CustomComboPreset.SMN_CarbuncleReminder;
            internal static bool carbyPresent = false;
            internal static DateTime noPetTime;
            internal static DateTime presentTime;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                
                // if (actionID is Ruin or Ruin2 or Ruin3 or DreadwyrmTrance or 星极超流 or 龙神迸发 or 灼热之光SearingLight or RadiantAegis or Outburst
                    // or Tridisaster or 宝石辉AoePreciousBrilliance or 宝石耀Gemshine)
                
                if (actionID is Ruin or Ruin2 or Ruin3 or DreadwyrmTrance or 星极超流 or 龙神迸发 or 灼热之光SearingLight or RadiantAegis or Outburst
                    or Tridisaster or 宝石辉AoePreciousBrilliance or 宝石耀Gemshine)
                {
                    presentTime = DateTime.Now;
                    int deltaTime = (presentTime - noPetTime).Milliseconds;
                    var gauge = GetJobGauge<SMNGauge>();

                    if (HasPetPresent())
                    {
                        carbyPresent = true;
                        noPetTime = DateTime.Now;
                    }

                    //Deals with the game's half second pet refresh
                    if (deltaTime > 500 && !HasPetPresent() && gauge.SummonTimerRemaining == 0 && gauge.Attunement == 0 &&
                        GetCooldownRemainingTime(Ruin) == 0)
                    {
                        carbyPresent = false;
                    }


                    if (carbyPresent == false && LocalPlayer.IsCasting == false)
                    {
                        //7.0改成自动召唤了
                        unsafe
                        {
                            ActionManager.Instance()->UseAction(ActionType.Action, SummonCarbuncle);
                        }
                        // return SummonCarbuncle;
                    }

                }

                return actionID;
            }
        }

        internal class SMN_Egi_AstralFlow : CustomCombo
        {
            protected internal override CustomComboPreset Preset => CustomComboPreset.SMN_Egi_AstralFlow;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if ((actionID is SummonTopaz_黄宝石召唤 or SummonTitan or SummonTitan2 or SummonEmerald_绿宝石召唤 or SummonGaruda or SummonGaruda2
                        or SummonRuby_红宝石召唤 or SummonIfrit or SummonIfrit2 && HasEffect(Buffs.TitansFavor)) ||
                    (actionID is SummonTopaz_黄宝石召唤 or SummonTitan or SummonTitan2 or SummonEmerald_绿宝石召唤 or SummonGaruda or SummonGaruda2 &&
                     HasEffect(Buffs.螺旋气流GarudasFavor)) ||
                    (actionID is SummonTopaz_黄宝石召唤 or SummonTitan or SummonTitan2 or SummonRuby_红宝石召唤 or SummonIfrit or SummonIfrit2 &&
                     (HasEffect(Buffs.IfritsFavor) || (lastComboMove == CrimsonCyclone && InMeleeRange()))))
                    return OriginalHook(星极超流);

                return actionID;
            }
        }

        internal class SMN_DemiAbilities : CustomCombo
        {
            protected internal override CustomComboPreset Preset => CustomComboPreset.SMN_DemiAbilities;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is 以太蓄能Aethercharge or DreadwyrmTrance or 龙神召唤SummonBahamut or 不死鸟召唤SummonPhoenix)
                {
                    if (CanSpellWeavePlus(actionID))
                    {
                        if (IsOffCooldown(龙神迸发) && GetCooldownRemainingTime(灼热之光SearingLight) is >= 10 && OriginalHook(Ruin) is 星极脉冲AstralImpulse)
                            return OriginalHook(龙神迸发);

                        if (IsOffCooldown(EnkindlePhoenix) && OriginalHook(Ruin) is 灵泉之炎FountainOfFire)
                            return OriginalHook(EnkindlePhoenix);

                        if ((OriginalHook(星极超流) is 死星核爆 && GetCooldownRemainingTime(灼热之光SearingLight) is >= 10 &&
                             IsOffCooldown(死星核爆)) ||
                            (OriginalHook(星极超流) is Rekindle && IsOffCooldown(Rekindle)))
                            return OriginalHook(星极超流);
                    }
                }

                return actionID;
            }
        }
    }
}