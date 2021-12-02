﻿using System.ComponentModel.DataAnnotations;
using AirSoft.Service.Common;
using AirSoft.Service.Exceptions;
using AirSoftApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace AirSoftApi.Controllers
{
    public class RootController : ControllerBase
    {
        private readonly ILogger _logger;

        public RootController(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected async Task<ServerResponseDto<TResponseDto>> HandleRequest<TRequestDto, TServiceRequest, TServiceResponse, TResponseDto>(
            Func<TServiceRequest, Task<TServiceResponse>> serviceFunction,
            TRequestDto requestDto,
            Func<TRequestDto, TServiceRequest> requestMap,
            Func<TServiceResponse, TResponseDto> responseMap,
            string logPath) where TRequestDto : IValidatableObject
        {
            _logger.Log(LogLevel.Trace, $"{logPath} started.");
            try
            {
                var validationResults = new List<ValidationResult>();
                if (!Validator.TryValidateObject(requestDto, new ValidationContext(requestDto), validationResults) || validationResults.Count > 0)
                {
                    var message = "";
                    if (validationResults.Count > 0)
                    {
                        message = string.Join("\r\n", validationResults.Select(vr => vr.ErrorMessage));
                    }
                    throw new AirSoftBaseException(ErrorCodes.InvalidParameters, "Invalid Parameters. \r\n" + message);
                }
                var serviceRequest = requestMap(requestDto);
                var serviceResponse = await serviceFunction(serviceRequest);
                var responseDto = responseMap(serviceResponse);
                _logger.Log(LogLevel.Trace, $"{logPath} ended.");
                return new ServerResponseDto<TResponseDto>(responseDto);
            }
            catch (AirSoftBaseException baseEx)
            {
                _logger.LogError(baseEx, $"{logPath} AirSoft Exception.");
                return new ServerResponseDto<TResponseDto>(new ErrorDto(baseEx.Code, baseEx.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{logPath} Common Exception.");
                return new ServerResponseDto<TResponseDto>(new ErrorDto(ErrorCodes.CommonError, ex.Message));
            }
        }
    }
}