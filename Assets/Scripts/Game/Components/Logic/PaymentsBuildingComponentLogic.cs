using EG.Core.AttributesSystem;
using EG.Core.Entity;
using EG.Core.Messages;
using UnityEngine;

namespace EG
{
    namespace Core.ComponentsSystem
    {

        //
        //store payments for a building in order to calculate the monthly payments (or whatever time was set for the building)
        //
        public class PaymentsBuildingComponentLogic : BaseComponent
        {

            private BuildingLogicData buildingLogicData = null;
            
            private uint initialMoneyAmount = 0;
            private uint currentMoney = 0;
            private uint tmpTotalSalaries = 0;
            private EG_MessageUpdatePayments messageUpdatePayments = null;
            private int totalDaysPassedPayments = 1;
            private int paymentDay = 0;
            
            public uint GetCurrentMoney => currentMoney;
            public uint GetTotalSalaries => tmpTotalSalaries;
            public uint GetInitialMoney => initialMoneyAmount;


            #region init
            
            public override void InitComponent(uint anEntityId, params object[] args)
            {
                messageUpdatePayments = new EG_MessageUpdatePayments();
                
                entityId = anEntityId;
                
                for (var i = 0; i < args.Length; ++i)
                {
                    if (args[i] is BuildingLogicData)
                    {
                        buildingLogicData = (BuildingLogicData) args[i];
                        break;
                    }
                }
                
                tmpTotalSalaries = 0;
                
                initialMoneyAmount = (uint)buildingLogicData.GetAttributeValue(Attribute_Enums.AttributeType.InitialMoneyAttr);
                currentMoney = initialMoneyAmount;
                
                totalDaysPassedPayments = 1;
                paymentDay = (int)buildingLogicData.GetAttributeValue(Attribute_Enums.AttributeType.PaymentDayAttr);
            }

            #endregion

            
            
            public override void Reset()
            {
                tmpTotalSalaries = 0;
                currentMoney = initialMoneyAmount;
            }


            public override void UpdateDayData()
            {
                totalDaysPassedPayments++;

                CheckMonthlyPayments();
            }
            
            
            #region public API

            public void SetTotalPaymentSalaries(uint anAmount)
            {
                tmpTotalSalaries += anAmount;
            }
            
            public void UpdateTotalMoney(uint anAmount)
            {
                if (currentMoney <= 0) return;
                
                currentMoney -= anAmount;
                currentMoney = (uint)Mathf.Clamp(currentMoney, 0, currentMoney);
            }

            public void SetTotalCosts(uint anAmount)
            {
                if (currentMoney <= 0) return;

                currentMoney -= anAmount;
                currentMoney = (uint) Mathf.Clamp(currentMoney, 0, currentMoney);
       
                messageUpdatePayments.SetData((uint)buildingLogicData.GetNameId, anAmount, entityId);

                EG_MessagesController<EG_MessageUpdatePayments>.Post(
                    (int)GameEnums.MessageTypes.UpdatePayments,
                    messageUpdatePayments);
            }

            public void SetMoneyEarned(uint anAmount)
            {
                currentMoney += anAmount;
            }

            public bool PaySalaries()
            {
                if (currentMoney <= 0) return false;
                
                messageUpdatePayments.SetData((uint)buildingLogicData.GetNameId, tmpTotalSalaries, entityId);

                EG_MessagesController<EG_MessageUpdatePayments>.Post(
                    (int)GameEnums.MessageTypes.UpdatePayments,
                    messageUpdatePayments);
                
                currentMoney -= tmpTotalSalaries;
                currentMoney = (uint)Mathf.Clamp(currentMoney, 0, currentMoney);
               
                tmpTotalSalaries = 0;
                
                return true;
            }

            #endregion
            
            
            
            

            
            #region do payments
            
            private void CheckMonthlyPayments()
            {
                if (paymentDay <= 0f) return;

                if (totalDaysPassedPayments < paymentDay) return;
                
                Debug.Log("PaymentsBuildingComponentLogic, pay day bitches!! " + buildingLogicData.GetNameId);
                totalDaysPassedPayments = 1;

                if (PaySalaries())
                {
                    SetNoPayments();
                }
                else
                {
                    ResetNoPayments();
                }
            }

            private void SetNoPayments()
            {
                var total = buildingLogicData.GetTotalEntities;
                
                for (var i = 0; i < total; ++i)
                {
                    var entity = buildingLogicData.GetEntityByPosition((uint)buildingLogicData.GetNameId, i);
      
                    Modifier modifier = AttributesAndModifiersController.CreateModifier(
                        IdGenerator.GetNewUId(),
                        entity.GetId,
                        -1,
                        false,
                        Attribute_Enums.AttributeFormulas.Addition,
                        1,
                        Attribute_Enums.AttributeType.DaysWithoutPaymentAttr,
                        false);
                
                    entity.AddModifier(modifier);
                }
            }

            private void ResetNoPayments()
            {
                var total = buildingLogicData.GetTotalEntities;
                
                for (var i = 0; i < total; ++i)
                {
                    var entity = buildingLogicData.GetEntityByPosition((uint)buildingLogicData.GetNameId, i);
                        
                    entity.RemoveAllModifiersWithType(Attribute_Enums.AttributeType.DaysWithoutPaymentAttr);
                } 
            }
            
            #endregion
            
        }

    }
}
