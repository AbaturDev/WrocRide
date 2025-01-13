﻿global using FluentValidation;
global using FluentValidation.AspNetCore;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.EntityFrameworkCore;
global using WrocRide.API.Entities;
global using WrocRide.API.Helpers;
global using WrocRide.API.Middleware;
global using WrocRide.Shared.DTOs.Car;
global using WrocRide.Shared.DTOs.Account;
global using WrocRide.Shared.DTOs.Document;
global using WrocRide.Shared.DTOs.Driver;
global using WrocRide.Shared.DTOs.Rating;
global using WrocRide.Shared.DTOs.Report;
global using WrocRide.Shared.DTOs.Ride;
global using WrocRide.Shared.DTOs.Schedule;
global using WrocRide.Shared.DTOs.User;
global using WrocRide.API.Validators;
global using WrocRide.API.Seeders;
global using WrocRide.API.Services;
global using Microsoft.IdentityModel.Tokens;
global using System.Text;
global using Microsoft.AspNetCore.Authorization;
global using WrocRide.API.Authorization;
global using Serilog;
global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;
global using WrocRide.API.Exceptions;
global using WrocRide.Shared.Enums;
global using Microsoft.AspNetCore.Mvc;
global using Bogus;
