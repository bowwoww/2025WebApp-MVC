using Microsoft.AspNetCore.Mvc;

namespace Program_C.Controllers
{
    public class IDNumberController : Controller
    {
        [HttpPost]
        public IActionResult IDNumberCheck (string ID)
        {
            int checkSum = 0;
            string letters = "ABCDEFGHJKLMNPQRSTUVXYWZIO";
            string firstLetter = (letters.IndexOf(ID[0]) + 10).ToString();

            if (ID.Length != 10)
                return Json("Invalid ID Number");

            ID = ID.Remove(0, 1);
            ID = ID.Insert(0, firstLetter);

            for (int i = 1; i < 10; i++)
            {
                checkSum += getNumber(ID[i]) * (10 - i);
            }
            checkSum = checkSum + getNumber(ID[0]) + getNumber(ID[10]);


            if (checkSum%10 == 0)
                return Json("Valid ID Number");
            return Json("Invalid ID Number");
        }

        private int getNumber(char c)
        {
            return (int)(c) - 48;
        }

    }
}
