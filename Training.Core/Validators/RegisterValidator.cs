using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.Core.DTOs;
using FluentValidation;
using System.Runtime.CompilerServices;
using System.Data;

namespace Training.Core.Validators
{
    public class RegisterValidator : AbstractValidator<RegisterRequestDTO>
    {
        public RegisterValidator() 
        { 
            this.RuleFor(x => x.Username)
                .NotEmpty()
                .WithMessage("帳號不可為空")
                .Length(6, 20)
                .WithMessage("帳號長度需在6~20字元之間");
            
            this.RuleFor(x => x.Password)
                .Matches(@"^(?=.*[A-Z])(?=.*\d).+$")
                .WithMessage("密碼必須包含至少一個大寫字母與數字");

            this.RuleFor(x => x.Age)
                .GreaterThanOrEqualTo(18)
                .WithMessage("年齡必須大於等於18");

            this.RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage("Email格式必須正確");
            
            
        }
    }
}
