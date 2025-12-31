using FluentEmail.Core;
using Microsoft.Extensions.Logging;

namespace SumterMartialArtsAzure.Server.Services;

public interface IEmailService
{
    Task SendBeltPromotionEmailAsync(
        string studentEmail,
        string studentName,
        string programName,
        string fromRank,
        string toRank,
        DateTime promotionDate,
        string instructorNotes);

    Task SendContactInfoUpdatedEmailAsync(string studentEmail, string studentName);

    Task SendProgramWithdrawalEmailAsync(
        string studentEmail,
        string studentName,
        string withdrawnProgram,
        List<string> remainingPrograms);

    Task SendSchoolWelcomeEmailAsync(string studentEmail, string studentName);

    Task SendProgramEnrollmentEmailAsync(
        string studentEmail,
        string studentName,
        string programName,
        string initialRank);

    Task SendPrivateLessonRequestConfirmationAsync(
        string studentEmail,
        string studentName,
        string instructorName,
        DateTime requestedDate);

    Task SendPrivateLessonApprovedAsync(
        string studentEmail,
        string studentName,
        string instructorName,
        DateTime scheduledDate);

    Task SendPrivateLessonRejectedAsync(
        string studentEmail,
        string studentName,
        string instructorName,
        DateTime requestedDate,
        string reason);

    Task SendPrivateLessonAdminNotificationAsync(
        string adminEmail,
        string studentName,
        string instructorName,
        DateTime requestedDate);
}

public class EmailService : IEmailService
{
    private readonly IFluentEmail _fluentEmail;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IFluentEmail fluentEmail, ILogger<EmailService> logger)
    {
        _fluentEmail = fluentEmail;
        _logger = logger;
    }

    public async Task SendSchoolWelcomeEmailAsync(string studentEmail, string studentName)
    {
        try
        {
            var subject = "Welcome to Sumter Martial Arts! 🥋";

            var body = $@"
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); 
                                  color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }}
                        .content {{ background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>Welcome to Sumter Martial Arts, {studentName}!</h1>
                        </div>
                        <div class='content'>
                            <p>We're thrilled to have you join our martial arts family!</p>
                            
                            <p>Your account has been created and you're ready to begin your journey with us.</p>

                            <h3>What's Next?</h3>
                            <ul>
                                <li><strong>Enroll in a Program</strong> - Choose from BJJ, Kickboxing, Judo, and more</li>
                                <li><strong>Check the Schedule</strong> - Find class times that work for you</li>
                                <li><strong>Visit the School</strong> - Stop by to meet the instructors and tour the facility</li>
                                <li><strong>Get Your Gear</strong> - We can help you get fitted for uniforms and equipment</li>
                            </ul>

                            <p><strong>Why Train With Us?</strong></p>
                            <ul>
                                <li>World-class instruction from experienced martial artists</li>
                                <li>Supportive community that celebrates your progress</li>
                                <li>Programs for all ages and skill levels</li>
                                <li>Focus on discipline, respect, and personal growth</li>
                            </ul>

                            <p>Whether you're here to learn self-defense, get in shape, compete, or simply challenge yourself, 
                               we're here to support you every step of the way.</p>

                            <p><strong>Questions?</strong> Don't hesitate to reach out - we're here to help!</p>

                            <p>We can't wait to see you on the mat!</p>

                            <div style='text-align: center; margin-top: 30px; color: #666; font-size: 14px;'>
                                <p>Sumter Martial Arts<br>
                                Your Journey Begins Here 🥋</p>
                            </div>
                        </div>
                    </div>
                </body>
                </html>
            ";

            var result = await _fluentEmail
                .To(studentEmail, studentName)
                .Subject(subject)
                .Body(body, isHtml: true)
                .SendAsync();

            if (result.Successful)
            {
                _logger.LogInformation("School welcome email sent to {Email}", studentEmail);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception sending school welcome email to {Email}", studentEmail);
        }
    }

    public async Task SendProgramEnrollmentEmailAsync( string studentEmail, string studentName, string programName, string initialRank)
    {
        try
        {
            var subject = $"Welcome to {programName}! 🥋";

            var body = $@"
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); 
                                  color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }}
                        .content {{ background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }}
                        .program-box {{ background: white; padding: 20px; margin: 20px 0; 
                                       border-left: 4px solid #667eea; border-radius: 5px; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>Welcome to {programName}!</h1>
                        </div>
                        <div class='content'>
                            <p>Hi {studentName},</p>
                            
                            <p>Congratulations on enrolling in <strong>{programName}</strong>! 
                               We're excited to have you join us on this martial arts journey.</p>
                            
                            <div class='program-box'>
                                <p><strong>Program:</strong> {programName}</p>
                                <p><strong>Starting Rank:</strong> {initialRank}</p>
                            </div>

                            <h3>What's Next?</h3>
                            <ul>
                                <li>Attend your first class (check the schedule)</li>
                                <li>Meet your instructors and fellow students</li>
                                <li>Get fitted for your uniform (if needed)</li>
                                <li>Learn the basic techniques and etiquette</li>
                                <li>Set your goals for progression</li>
                            </ul>

                            <p><strong>Remember:</strong> Everyone starts as a beginner. Focus on learning, 
                               stay consistent, and enjoy the process. Your instructors and teammates are here to support you!</p>

                            <p>See you on the mat!</p>

                            <div style='text-align: center; margin-top: 30px; color: #666; font-size: 14px;'>
                                <p>Sumter Martial Arts<br>
                                Your journey begins now! 🥋</p>
                            </div>
                        </div>
                    </div>
                </body>
                </html>
            ";

            var result = await _fluentEmail
                .To(studentEmail, studentName)
                .Subject(subject)
                .Body(body, isHtml: true)
                .SendAsync();

            if (result.Successful)
            {
                _logger.LogInformation("Program enrollment email sent to {Email} for {Program}", studentEmail, programName);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception sending program enrollment email to {Email}", studentEmail);
        }
    }

    public async Task SendBeltPromotionEmailAsync(
        string studentEmail,
        string studentName,
        string programName,
        string fromRank,
        string toRank,
        DateTime promotionDate,
        string instructorNotes)
    {
        try
        {
            var subject = $"🥋 Congratulations on Your {toRank} Promotion!";

            var body = $@"
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); 
                                  color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }}
                        .content {{ background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }}
                        .promotion-box {{ background: white; padding: 20px; margin: 20px 0; 
                                         border-left: 4px solid #667eea; border-radius: 5px; }}
                        .rank {{ font-size: 24px; font-weight: bold; color: #667eea; }}
                        .footer {{ text-align: center; margin-top: 30px; color: #666; font-size: 14px; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>🎉 Congratulations, {studentName}!</h1>
                        </div>
                        <div class='content'>
                            <p>We are thrilled to inform you that you have been promoted in {programName}!</p>
                            
                            <div class='promotion-box'>
                                <p><strong>From:</strong> <span class='rank'>{fromRank}</span></p>
                                <p><strong>To:</strong> <span class='rank'>{toRank}</span></p>
                                <p><strong>Date:</strong> {promotionDate:MMMM dd, yyyy}</p>
                            </div>

                            <h3>Instructor Feedback:</h3>
                            <p style='background: white; padding: 15px; border-radius: 5px; font-style: italic;'>
                                ""{instructorNotes}""
                            </p>

                            <p>This achievement is a testament to your hard work, dedication, and perseverance. 
                               Continue to train with the same passion and discipline!</p>

                            <p><strong>What's Next?</strong></p>
                            <ul>
                                <li>Continue attending classes regularly</li>
                                <li>Practice the new techniques you've learned</li>
                                <li>Help mentor newer students</li>
                                <li>Set your sights on the next rank!</li>
                            </ul>

                            <p>We're proud of your progress and look forward to seeing you continue to grow!</p>

                            <div class='footer'>
                                <p>Sumter Martial Arts<br>
                                Keep training, keep growing! 🥋</p>
                            </div>
                        </div>
                    </div>
                </body>
                </html>
            ";

            var result = await _fluentEmail
                .To(studentEmail, studentName)
                .Subject(subject)
                .Body(body, isHtml: true)
                .SendAsync();

            if (result.Successful)
            {
                _logger.LogInformation(
                    "Promotion email sent successfully to {Email} for {Rank} promotion",
                    studentEmail,
                    toRank
                );
            }
            else
            {
                _logger.LogError(
                    "Failed to send promotion email to {Email}: {Errors}",
                    studentEmail,
                    string.Join(", ", result.ErrorMessages)
                );
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Exception sending promotion email to {Email}",
                studentEmail
            );
        }
    }

    public async Task SendContactInfoUpdatedEmailAsync(string studentEmail, string studentName)
    {
        try
        {
            var subject = "Your Contact Information Has Been Updated";

            var body = $@"
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background: linear-gradient(135deg, #4facfe 0%, #00f2fe 100%); 
                                  color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }}
                        .content {{ background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }}
                        .info-box {{ background: #e3f2fd; padding: 15px; border-left: 4px solid #2196f3; 
                                    border-radius: 5px; margin: 20px 0; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>Contact Information Updated</h1>
                        </div>
                        <div class='content'>
                            <p>Hi {studentName},</p>
                            
                            <p>This email confirms that your contact information has been successfully updated in our system.</p>

                            <div class='info-box'>
                                <strong>Security Notice:</strong><br>
                                If you did not request this change, please contact us immediately at the school.
                            </div>

                            <p>Your updated information will be used for:</p>
                            <ul>
                                <li>Important announcements and updates</li>
                                <li>Belt promotion notifications</li>
                                <li>Schedule changes</li>
                                <li>Emergency communications</li>
                            </ul>

                            <p>If you need to make any additional changes, please contact us or update your information through your student portal.</p>

                            <p>Thank you for keeping your information up to date!</p>

                            <div style='text-align: center; margin-top: 30px; color: #666; font-size: 14px;'>
                                <p>Sumter Martial Arts<br>
                                Training with Excellence 🥋</p>
                            </div>
                        </div>
                    </div>
                </body>
                </html>
            ";

            var result = await _fluentEmail
                .To(studentEmail, studentName)
                .Subject(subject)
                .Body(body, isHtml: true)
                .SendAsync();

            if (result.Successful)
            {
                _logger.LogInformation("Contact info update confirmation sent to {Email}", studentEmail);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception sending contact info update email to {Email}", studentEmail);
        }
    }

    public async Task SendProgramWithdrawalEmailAsync(
        string studentEmail,
        string studentName,
        string withdrawnProgram,
        List<string> remainingPrograms)
    {
        try
        {
            var subject = $"You've Been Withdrawn from {withdrawnProgram}";
            var hasRemainingPrograms = remainingPrograms.Any();
            var remainingProgramsList = hasRemainingPrograms
                ? string.Join("</li><li>", remainingPrograms)
                : "";

            var body = $@"
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background: linear-gradient(135deg, #fa709a 0%, #fee140 100%); 
                                  color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }}
                        .content {{ background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }}
                        .program-box {{ background: white; padding: 20px; margin: 20px 0; 
                                       border-left: 4px solid #fa709a; border-radius: 5px; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>Program Withdrawal Confirmed</h1>
                        </div>
                        <div class='content'>
                            <p>Hi {studentName},</p>
                            
                            <p>This email confirms that you have been withdrawn from <strong>{withdrawnProgram}</strong>.</p>

                            <div class='program-box'>
                                <p><strong>Withdrawn Program:</strong> {withdrawnProgram}</p>
                            </div>

                            {(hasRemainingPrograms ? $@"
                                <p>You are still actively enrolled in:</p>
                                <ul>
                                    <li>{remainingProgramsList}</li>
                                </ul>
                                <p>We look forward to continuing your training in {(remainingPrograms.Count == 1 ? "this program" : "these programs")}!</p>
                            " : $@"
                                <p>You are no longer enrolled in any programs at Sumter Martial Arts.</p>
                                <p>We understand that life circumstances change. If you'd like to return in the future, 
                                   we'd be happy to welcome you back!</p>
                            ")}

                            <p><strong>If this withdrawal was made in error,</strong> please contact us immediately 
                               and we'll be happy to re-enroll you.</p>

                            {(hasRemainingPrograms ? $@"
                                <p>Keep up the great work in your remaining program{(remainingPrograms.Count > 1 ? "s" : "")}!</p>
                            " : $@"
                                <p>Thank you for being part of our martial arts family. We hope to see you on the mat again soon!</p>
                            ")}

                            <div style='text-align: center; margin-top: 30px; color: #666; font-size: 14px;'>
                                <p>Sumter Martial Arts<br>
                                Once a student, always family 🥋</p>
                            </div>
                        </div>
                    </div>
                </body>
                </html>
            ";

            var result = await _fluentEmail
                .To(studentEmail, studentName)
                .Subject(subject)
                .Body(body, isHtml: true)
                .SendAsync();

            if (result.Successful)
            {
                _logger.LogInformation("Program withdrawal email sent to {Email} for {Program}", studentEmail, withdrawnProgram);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception sending withdrawal email to {Email}", studentEmail);
        }
    }

    // Add these implementations to your EmailService class:

    public async Task SendPrivateLessonRequestConfirmationAsync(
        string studentEmail,
        string studentName,
        string instructorName,
        DateTime requestedDate)
    {
        try
        {
            var subject = "Private Lesson Request Received 📅";

            var body = $@"
            <html>
            <head>
                <style>
                    body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                    .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                    .header {{ background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); 
                              color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }}
                    .content {{ background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }}
                    .info-box {{ background: white; padding: 20px; margin: 20px 0; 
                                border-left: 4px solid #667eea; border-radius: 5px; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h1>Request Received!</h1>
                    </div>
                    <div class='content'>
                        <p>Hi {studentName},</p>
                        
                        <p>We've received your private lesson request. Here are the details:</p>

                        <div class='info-box'>
                            <p><strong>Instructor:</strong> {instructorName}</p>
                            <p><strong>Requested Date:</strong> {requestedDate:MMMM dd, yyyy 'at' h:mm tt}</p>
                        </div>

                        <p><strong>What's Next?</strong></p>
                        <ul>
                            <li>Your instructor will review the request</li>
                            <li>You'll receive a confirmation email once approved</li>
                            <li>If the requested time doesn't work, we'll suggest alternatives</li>
                        </ul>

                        <p>We'll get back to you within 24 hours!</p>

                        <div style='text-align: center; margin-top: 30px; color: #666; font-size: 14px;'>
                            <p>Sumter Martial Arts<br>
                            Personalized Training Excellence 🥋</p>
                        </div>
                    </div>
                </div>
            </body>
            </html>
        ";

            var result = await _fluentEmail
                .To(studentEmail, studentName)
                .Subject(subject)
                .Body(body, isHtml: true)
                .SendAsync();

            if (result.Successful)
            {
                _logger.LogInformation("Private lesson request confirmation sent to {Email}", studentEmail);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception sending private lesson confirmation to {Email}", studentEmail);
        }
    }

    public async Task SendPrivateLessonApprovedAsync(
        string studentEmail,
        string studentName,
        string instructorName,
        DateTime scheduledDate)
    {
        try
        {
            var subject = "✅ Your Private Lesson is Confirmed!";

            var body = $@"
            <html>
            <head>
                <style>
                    body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                    .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                    .header {{ background: linear-gradient(135deg, #11998e 0%, #38ef7d 100%); 
                              color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }}
                    .content {{ background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }}
                    .confirmed-box {{ background: #d4edda; padding: 20px; margin: 20px 0; 
                                     border-left: 4px solid #28a745; border-radius: 5px; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h1>🎉 Your Lesson is Confirmed!</h1>
                    </div>
                    <div class='content'>
                        <p>Great news, {studentName}!</p>
                        
                        <p>Your private lesson has been approved and scheduled.</p>

                        <div class='confirmed-box'>
                            <p><strong>Instructor:</strong> {instructorName}</p>
                            <p><strong>Date & Time:</strong> {scheduledDate:MMMM dd, yyyy 'at' h:mm tt}</p>
                        </div>

                        <p><strong>Before Your Lesson:</strong></p>
                        <ul>
                            <li>Arrive 10 minutes early to warm up</li>
                            <li>Bring water and any necessary equipment</li>
                            <li>Come prepared with questions or areas you want to focus on</li>
                            <li>Add this to your calendar so you don't forget!</li>
                        </ul>

                        <p>This is a great opportunity for one-on-one instruction. Make the most of it!</p>

                        <p><strong>Need to reschedule?</strong> Please contact us at least 24 hours in advance.</p>

                        <div style='text-align: center; margin-top: 30px; color: #666; font-size: 14px;'>
                            <p>Sumter Martial Arts<br>
                            See you on the mat! 🥋</p>
                        </div>
                    </div>
                </div>
            </body>
            </html>
        ";

            var result = await _fluentEmail
                .To(studentEmail, studentName)
                .Subject(subject)
                .Body(body, isHtml: true)
                .SendAsync();

            if (result.Successful)
            {
                _logger.LogInformation("Private lesson approved email sent to {Email}", studentEmail);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception sending private lesson approval to {Email}", studentEmail);
        }
    }

    public async Task SendPrivateLessonRejectedAsync(
        string studentEmail,
        string studentName,
        string instructorName,
        DateTime requestedDate,
        string reason)
    {
        try
        {
            var subject = "Private Lesson Request - Alternative Times Available";

            var body = $@"
            <html>
            <head>
                <style>
                    body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                    .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                    .header {{ background: linear-gradient(135deg, #f093fb 0%, #f5576c 100%); 
                              color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }}
                    .content {{ background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }}
                    .reason-box {{ background: #fff3cd; padding: 15px; margin: 20px 0; 
                                  border-left: 4px solid #ffc107; border-radius: 5px; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h1>About Your Private Lesson Request</h1>
                    </div>
                    <div class='content'>
                        <p>Hi {studentName},</p>
                        
                        <p>Thank you for your interest in a private lesson with {instructorName}.</p>

                        <p>Unfortunately, the time you requested ({requestedDate:MMMM dd, yyyy 'at' h:mm tt}) is not available.</p>

                        <div class='reason-box'>
                            <strong>Reason:</strong> {reason}
                        </div>

                        <p><strong>We'd still love to schedule you!</strong></p>
                        <ul>
                            <li>Contact us to discuss alternative times</li>
                            <li>Check with other available instructors</li>
                            <li>Submit a new request with different dates</li>
                        </ul>

                        <p>Private lessons are a great investment in your training, and we want to make sure 
                           you get the personalized attention you deserve.</p>

                        <p>Please reach out and we'll find a time that works!</p>

                        <div style='text-align: center; margin-top: 30px; color: #666; font-size: 14px;'>
                            <p>Sumter Martial Arts<br>
                            We're here to help! 🥋</p>
                        </div>
                    </div>
                </div>
            </body>
            </html>
        ";

            var result = await _fluentEmail
                .To(studentEmail, studentName)
                .Subject(subject)
                .Body(body, isHtml: true)
                .SendAsync();

            if (result.Successful)
            {
                _logger.LogInformation("Private lesson rejection email sent to {Email}", studentEmail);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception sending private lesson rejection to {Email}", studentEmail);
        }
    }

    public async Task SendPrivateLessonAdminNotificationAsync(
        string adminEmail,
        string studentName,
        string instructorName,
        DateTime requestedDate)
    {
        try
        {
            var subject = $"🔔 New Private Lesson Request - {studentName}";

            var body = $@"
            <html>
            <head>
                <style>
                    body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                    .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                    .header {{ background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); 
                              color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }}
                    .content {{ background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }}
                    .request-box {{ background: white; padding: 20px; margin: 20px 0; 
                                   border: 2px solid #667eea; border-radius: 5px; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h1>New Private Lesson Request</h1>
                    </div>
                    <div class='content'>
                        <p>A new private lesson request has been submitted.</p>

                        <div class='request-box'>
                            <p><strong>Student:</strong> {studentName}</p>
                            <p><strong>Requested Instructor:</strong> {instructorName}</p>
                            <p><strong>Requested Date:</strong> {requestedDate:MMMM dd, yyyy 'at' h:mm tt}</p>
                        </div>

                        <p><strong>Action Required:</strong> Review and approve/reject this request in the admin panel.</p>

                        <div style='text-align: center; margin-top: 30px; color: #666; font-size: 14px;'>
                            <p>Sumter Martial Arts Admin Portal</p>
                        </div>
                    </div>
                </div>
            </body>
            </html>
        ";

            var result = await _fluentEmail
                .To(adminEmail, "Admin")
                .Subject(subject)
                .Body(body, isHtml: true)
                .SendAsync();

            if (result.Successful)
            {
                _logger.LogInformation("Admin notification sent for private lesson request");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception sending admin notification");
        }
    }
}