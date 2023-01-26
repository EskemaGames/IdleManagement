using System.Collections.Generic;
using EG.Core.Entity;
using UnityEngine;

namespace EG
{
    namespace Core.Components
    {

        public class PaymentsComponentLogic : BaseComponentLogic
        {

            private BuildingLogicData buildingLogicData = null;
            
            private uint initialMoneyAmount = 0;
            private uint currentMoney = 0;
            private uint tmpTotalPayments = 0;
            
            public uint GetCurrentMoney => currentMoney;
            public uint GetTotalPayments => tmpTotalPayments;
            public uint GetInitialMoney => initialMoneyAmount;
            
            

            #region init

            public override void InitComponent(EntityLogicData aLogicData, List<BaseComponentLogic> aLogicComponents)
            {
                currentMoney = 0;
                tmpTotalPayments = 0;
                
                if (aLogicData == null) return;
                
                entityId = aLogicData.GetId;
                initialMoneyAmount = (uint)aLogicData.GetAttributeValue(Attribute_Enums.AttributeType.InitialMoneyAttr);
            }

            #endregion


            public override void SetData(params object[] args)
            {
                for (var i = 0; i < args.Length; ++i)
                {
                    if (args[i] is BuildingLogicData)
                    {
                        buildingLogicData = (BuildingLogicData) args[i];
                    }
                }
            }
            

            
            public override void Reset()
            {
                tmpTotalPayments = 0;
            }
            
            
            
            #region public API

            public void SetTotalPayments(uint anAmount)
            {
                tmpTotalPayments += anAmount;
            }

            public void SetCurrentMoney()
            {
                currentMoney -= (uint)Mathf.Clamp(currentMoney, 0, tmpTotalPayments);
            }

            public void SetMoneyEarned(uint anAmount)
            {
                currentMoney += anAmount;
            }
            
            #endregion
            
        }

    }
}
