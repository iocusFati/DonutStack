using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gameplay.DonutFolder.DonutStackFolder
{
    public static class DonutStickExtension
    {
        public static void TransferTo(this Stack<Donut> mainStack, Stack<Donut> transferStack,
            int transferAmount, int maxStackCount, out int donutsTransferred)
        {
            // for (int donutCount = 0; donutCount < transferAmount; donutCount++)
            // {
            //     if (transferStack.Count > maxStackCount)
            //         return;
            //     
            //     if (mainStack.Count > maxStackCount) 
            //         Debug.LogError($"Main Stack count is bigger then max: {maxStackCount}");
            //     if (transferStack.Count > maxStackCount - transferAmount)
            //         Debug.LogError($"Transfer Stack count {transferStack.Count} is bigger then possible: {maxStackCount - transferAmount}");
            //     
            //     Donut topDonut = mainStack.Pop();
            //     transferStack.Push(topDonut);
            // }

            donutsTransferred = 0;
            while (transferStack.Count < maxStackCount 
                   && transferAmount > 0)
            {
                if (transferStack.Count > maxStackCount)
                    return;
                if (mainStack.Count > maxStackCount) 
                    Debug.LogError($"Main Stack count is bigger then max: {maxStackCount}");
                if (transferStack.Count >= maxStackCount - transferAmount)
                    Debug.LogError($"Transfer Stack count {transferStack.Count} is bigger then possible: {maxStackCount - transferAmount}");
                
                Donut topDonut = mainStack.Pop();
                transferStack.Push(topDonut);

                donutsTransferred++;
                transferAmount--;
                Debug.Log($"Donuts Transferred {donutsTransferred}");
            }
        }
        
        public static int GetDonutNumberOfType(this Stack<Donut> stack, DonutType topType)
        {
            for (int donutCount = 1; donutCount < stack.Count; donutCount++)
            {
                if (stack.ElementAt(donutCount).Type == topType)
                {
                    continue;
                }

                return donutCount;
            }

            return default;
        }
        
        public static int GetDonutNumberOfType(this Stack<Donut> stack)
        {
            DonutType topType = stack.GetTopDonutType();
            int donutCount;
            for (donutCount = 1; donutCount < stack.Count; donutCount++)
            {
                if (stack.ElementAt(donutCount).Type == topType)
                {
                    continue;
                }

                return donutCount;
            }

            return donutCount;
        }
        
        public static DonutType GetTopDonutType(this Stack<Donut> stack) => 
            stack.Peek().Type;
    }
}