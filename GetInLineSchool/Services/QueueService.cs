using GetInLineSchool.DTOs.Request;
using GetInLineSchool.DTOs.Response;
using GetInLineSchool.Models;
using GetInLineSchool.Repositories;
using Microsoft.Data.SqlClient;
using Org.BouncyCastle.Asn1.Cms.Ecc;
using System.Security.Cryptography;

namespace GetInLineSchool.Services
{
    public class QueueService
    {
        private readonly QueueRepository _repository = new QueueRepository();

        public async Task<(StatusCodes Code, string? Message, CreateQueueResponse? Response)> CreateQueueAsync(CreateQueueRequest request, int idCreator, string role)
        {
            try
            {
                Queue queue = new Queue()
                {
                    IDTeacher = role != "5" ? idCreator : null,
                    IDStudent = role == "5" ? idCreator : null,
                    IDClass = request.IDClass,
                    Subject = request.Subject,
                    Duration = request.Duration
                };

                if (role == "5")
                {
                    var check = await _repository.CheckStudentClassAndPermissionAsync(idCreator);

                    if (!check.Permission)
                        return (StatusCodes.Forbidden, "At your school students have not permission to create queues.", null);

                    if (check.IDClass != request.IDClass)
                        return (StatusCodes.Forbidden, "As a student you can create queue only for your class.", null);

                    queue.IDSchool = check.IDSchool;
                }
                else if (role == "2" || role == "3")
                {
                    if (request.IDClass != null)
                    {
                        var check = await _repository.CheckTeacherSchoolAsync(idCreator, request.IDClass);
                        if (check.TeacherSchool != check.ClassSchool)
                            return (StatusCodes.Forbidden, "You can create queues only for classes at your school.", null);

                        queue.IDSchool = check.TeacherSchool;
                    }
                    else
                        queue.IDSchool = await _repository.GetTeacherSchoolAsync(idCreator);
                }
                else
                    return (StatusCodes.Forbidden, "You have not permission to create queues.", null);

                queue.Code = await GenerateCode();

                var createResult = await _repository.CreateQueueAsync(queue);

                return createResult == 0 ? (StatusCodes.ServerError, "Failed to create queue.", null)
                    : (StatusCodes.Success, null, new CreateQueueResponse { Code = queue.Code });
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                return (StatusCodes.Conflict, "A record with this unique value already exists.", null);
            }
            catch (Exception ex)
            {
                return (StatusCodes.ServerError, "SQL error occured.", null);
            }
        }

        private async Task<string> GenerateCode()
        {
            string code = "";

            while(true)
            {
                code = HelperService.GenerateSignUpCodes();
                if (!await _repository.IsActiveCodeAsync(code))
                    break;
            }

            return code;
        }
    }
}
