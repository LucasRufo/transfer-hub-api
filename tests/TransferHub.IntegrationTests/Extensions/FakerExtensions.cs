﻿using Bogus;
using TransferHub.Infrastructure;

namespace TransferHub.IntegrationTests.Extensions;

public static class FakerExtensions
{
    public static T GenerateInDatabase<T>(this Faker<T> objFake, TransferHubContext context)
        where T : class
    {
        var obj = objFake.Generate();

        context.Add(obj);
        context.SaveChanges();
        return obj;
    }

    public static List<T> GenerateInDatabase<T>(this Faker<T> objFake, TransferHubContext context, int count)
        where T : class
    {
        var objList = objFake.Generate(count);

        context.AddRange(objList);
        context.SaveChanges();
        return objList;
    }
}