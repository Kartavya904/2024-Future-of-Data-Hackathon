using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;

public class IndexModel : PageModel
{
    public List<int> List1 { get; set; }
    public List<int> List2 { get; set; }
    public List<int> List3 { get; set; }
    public List<int> List4 { get; set; }  // Fourth dimension

    public void OnGet()
    {
        Random random = new Random();
        List1 = new List<int>();
        List2 = new List<int>();
        List3 = new List<int>();
        List4 = new List<int>();

        for (int i = 0; i < 20; i++)
        {
            List1.Add(random.Next(1, 101));
            List2.Add(random.Next(1, 101));
            List3.Add(random.Next(1, 101));
            List4.Add(random.Next(10, 50));  // Different range for variation
        }
    }
}
