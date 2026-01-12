using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SumterMartialArtsAzure.Server.Domain;

namespace SumterMartialArtsAzure.Server.DataAccess.Configuration;

public class EmailTemplateConfiguration : IEntityTypeConfiguration<EmailTemplate>
{
    public void Configure(EntityTypeBuilder<EmailTemplate> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.TemplateKey)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Subject)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(e => e.Body)
            .IsRequired()
            .HasColumnType("nvarchar(max)");

        builder.Property(e => e.Description)
            .HasMaxLength(1000);

        builder.Property(e => e.IsActive)
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.Property(e => e.UpdatedAt);

        // Unique constraint on TemplateKey
        builder.HasIndex(e => e.TemplateKey)
            .IsUnique();

        // Seed default templates
        builder.HasData(
            // 1. School Welcome Email
            new
            {
                Id = 1,
                TemplateKey = "SchoolWelcome",
                Name = "School Welcome Email",
                Subject = "Welcome to Sumter Martial Arts! 🥋",
                Body = @"<html>
<head>
    <style>
        body { font-family: Arial, sans-serif; line-height: 1.6; color: #333; }
        .container { max-width: 600px; margin: 0 auto; padding: 20px; }
        .header { background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); 
                  color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }
        .content { background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Welcome to Sumter Martial Arts, {{StudentName}}!</h1>
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
                <p>Sumter Martial Arts<br>Your Journey Begins Here 🥋</p>
            </div>
        </div>
    </div>
</body>
</html>",
                Description = "Sent to new students when they first join the school. Variables: {{StudentName}}",
                IsActive = true,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },

            // 2. Program Enrollment Email
            new
            {
                Id = 2,
                TemplateKey = "ProgramEnrollment",
                Name = "Program Enrollment Email",
                Subject = "Welcome to {{ProgramName}}! 🥋",
                Body = @"<html>
<head>
    <style>
        body { font-family: Arial, sans-serif; line-height: 1.6; color: #333; }
        .container { max-width: 600px; margin: 0 auto; padding: 20px; }
        .header { background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); 
                  color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }
        .content { background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }
        .program-box { background: white; padding: 20px; margin: 20px 0; 
                       border-left: 4px solid #667eea; border-radius: 5px; }
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Welcome to {{ProgramName}}!</h1>
        </div>
        <div class='content'>
            <p>Hi {{StudentName}},</p>
            <p>Congratulations on enrolling in <strong>{{ProgramName}}</strong>! 
               We're excited to have you join us on this martial arts journey.</p>
            <div class='program-box'>
                <p><strong>Program:</strong> {{ProgramName}}</p>
                <p><strong>Starting Rank:</strong> {{InitialRank}}</p>
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
                <p>Sumter Martial Arts<br>Your journey begins now! 🥋</p>
            </div>
        </div>
    </div>
</body>
</html>",
                Description = "Sent when a student enrolls in a new program. Variables: {{StudentName}}, {{ProgramName}}, {{InitialRank}}",
                IsActive = true,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },

            // 3. Belt Promotion Email
            new
            {
                Id = 3,
                TemplateKey = "BeltPromotion",
                Name = "Belt Promotion Email",
                Subject = "🥋 Congratulations on Your {{ToRank}} Promotion!",
                Body = @"<html>
<head>
    <style>
        body { font-family: Arial, sans-serif; line-height: 1.6; color: #333; }
        .container { max-width: 600px; margin: 0 auto; padding: 20px; }
        .header { background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); 
                  color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }
        .content { background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }
        .promotion-box { background: white; padding: 20px; margin: 20px 0; 
                         border-left: 4px solid #667eea; border-radius: 5px; }
        .rank { font-size: 24px; font-weight: bold; color: #667eea; }
        .footer { text-align: center; margin-top: 30px; color: #666; font-size: 14px; }
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>🎉 Congratulations, {{StudentName}}!</h1>
        </div>
        <div class='content'>
            <p>We are thrilled to inform you that you have been promoted in {{ProgramName}}!</p>
            <div class='promotion-box'>
                <p><strong>From:</strong> <span class='rank'>{{FromRank}}</span></p>
                <p><strong>To:</strong> <span class='rank'>{{ToRank}}</span></p>
                <p><strong>Date:</strong> {{PromotionDate}}</p>
            </div>
            <h3>Instructor Feedback:</h3>
            <p style='background: white; padding: 15px; border-radius: 5px; font-style: italic;'>
                ""{{InstructorNotes}}""
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
                <p>Sumter Martial Arts<br>Keep training, keep growing! 🥋</p>
            </div>
        </div>
    </div>
</body>
</html>",
                Description = "Sent when a student is promoted to a new rank. Variables: {{StudentName}}, {{ProgramName}}, {{FromRank}}, {{ToRank}}, {{PromotionDate}}, {{InstructorNotes}}",
                IsActive = true,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },

            // 4. Contact Info Updated Email
            new
            {
                Id = 4,
                TemplateKey = "ContactInfoUpdated",
                Name = "Contact Information Updated",
                Subject = "Your Contact Information Has Been Updated",
                Body = @"<html>
<head>
    <style>
        body { font-family: Arial, sans-serif; line-height: 1.6; color: #333; }
        .container { max-width: 600px; margin: 0 auto; padding: 20px; }
        .header { background: linear-gradient(135deg, #4facfe 0%, #00f2fe 100%); 
                  color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }
        .content { background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }
        .info-box { background: #e3f2fd; padding: 15px; border-left: 4px solid #2196f3; 
                    border-radius: 5px; margin: 20px 0; }
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Contact Information Updated</h1>
        </div>
        <div class='content'>
            <p>Hi {{StudentName}},</p>
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
                <p>Sumter Martial Arts<br>Training with Excellence 🥋</p>
            </div>
        </div>
    </div>
</body>
</html>",
                Description = "Sent when a student updates their contact information. Variables: {{StudentName}}",
                IsActive = true,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },

            // 5. Program Withdrawal Email
            new
            {
                Id = 5,
                TemplateKey = "ProgramWithdrawal",
                Name = "Program Withdrawal Email",
                Subject = "You've Been Withdrawn from {{WithdrawnProgram}}",
                Body = @"<html>
<head>
    <style>
        body { font-family: Arial, sans-serif; line-height: 1.6; color: #333; }
        .container { max-width: 600px; margin: 0 auto; padding: 20px; }
        .header { background: linear-gradient(135deg, #fa709a 0%, #fee140 100%); 
                  color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }
        .content { background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }
        .program-box { background: white; padding: 20px; margin: 20px 0; 
                       border-left: 4px solid #fa709a; border-radius: 5px; }
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Program Withdrawal Confirmed</h1>
        </div>
        <div class='content'>
            <p>Hi {{StudentName}},</p>
            <p>This email confirms that you have been withdrawn from <strong>{{WithdrawnProgram}}</strong>.</p>
            <div class='program-box'>
                <p><strong>Withdrawn Program:</strong> {{WithdrawnProgram}}</p>
            </div>
            {{#if HasRemainingPrograms}}
                <p>You are still actively enrolled in:</p>
                <ul>
                    {{#each RemainingPrograms}}
                    <li>{{this}}</li>
                    {{/each}}
                </ul>
                <p>We look forward to continuing your training in these programs!</p>
            {{else}}
                <p>You are no longer enrolled in any programs at Sumter Martial Arts.</p>
                <p>We understand that life circumstances change. If you'd like to return in the future, 
                   we'd be happy to welcome you back!</p>
            {{/if}}
            <p><strong>If this withdrawal was made in error,</strong> please contact us immediately 
               and we'll be happy to re-enroll you.</p>
            <div style='text-align: center; margin-top: 30px; color: #666; font-size: 14px;'>
                <p>Sumter Martial Arts<br>Once a student, always family 🥋</p>
            </div>
        </div>
    </div>
</body>
</html>",
                Description = "Sent when a student is withdrawn from a program. Variables: {{StudentName}}, {{WithdrawnProgram}}, {{HasRemainingPrograms}}, {{RemainingPrograms}}",
                IsActive = true,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },

            // 6. Private Lesson Request Confirmation
            new
            {
                Id = 6,
                TemplateKey = "PrivateLessonRequestConfirmation",
                Name = "Private Lesson Request Confirmation",
                Subject = "Private Lesson Request Received 📅",
                Body = @"<html>
<head>
    <style>
        body { font-family: Arial, sans-serif; line-height: 1.6; color: #333; }
        .container { max-width: 600px; margin: 0 auto; padding: 20px; }
        .header { background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); 
                  color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }
        .content { background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }
        .info-box { background: white; padding: 20px; margin: 20px 0; 
                    border-left: 4px solid #667eea; border-radius: 5px; }
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Request Received!</h1>
        </div>
        <div class='content'>
            <p>Hi {{StudentName}},</p>
            <p>We've received your private lesson request. Here are the details:</p>
            <div class='info-box'>
                <p><strong>Instructor:</strong> {{InstructorName}}</p>
                <p><strong>Requested Date:</strong> {{RequestedDate}}</p>
            </div>
            <p><strong>What's Next?</strong></p>
            <ul>
                <li>Your instructor will review the request</li>
                <li>You'll receive a confirmation email once approved</li>
                <li>If the requested time doesn't work, we'll suggest alternatives</li>
            </ul>
            <p>We'll get back to you within 24 hours!</p>
            <div style='text-align: center; margin-top: 30px; color: #666; font-size: 14px;'>
                <p>Sumter Martial Arts<br>Personalized Training Excellence 🥋</p>
            </div>
        </div>
    </div>
</body>
</html>",
                Description = "Sent when a student submits a private lesson request. Variables: {{StudentName}}, {{InstructorName}}, {{RequestedDate}}",
                IsActive = true,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },

            // 7. Private Lesson Approved
            new
            {
                Id = 7,
                TemplateKey = "PrivateLessonApproved",
                Name = "Private Lesson Approved",
                Subject = "✅ Your Private Lesson is Confirmed!",
                Body = @"<html>
<head>
    <style>
        body { font-family: Arial, sans-serif; line-height: 1.6; color: #333; }
        .container { max-width: 600px; margin: 0 auto; padding: 20px; }
        .header { background: linear-gradient(135deg, #11998e 0%, #38ef7d 100%); 
                  color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }
        .content { background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }
        .confirmed-box { background: #d4edda; padding: 20px; margin: 20px 0; 
                         border-left: 4px solid #28a745; border-radius: 5px; }
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>🎉 Your Lesson is Confirmed!</h1>
        </div>
        <div class='content'>
            <p>Great news, {{StudentName}}!</p>
            <p>Your private lesson has been approved and scheduled.</p>
            <div class='confirmed-box'>
                <p><strong>Instructor:</strong> {{InstructorName}}</p>
                <p><strong>Date & Time:</strong> {{ScheduledDate}}</p>
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
                <p>Sumter Martial Arts<br>See you on the mat! 🥋</p>
            </div>
        </div>
    </div>
</body>
</html>",
                Description = "Sent when a private lesson request is approved. Variables: {{StudentName}}, {{InstructorName}}, {{ScheduledDate}}",
                IsActive = true,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },

            // 8. Private Lesson Rejected
            new
            {
                Id = 8,
                TemplateKey = "PrivateLessonRejected",
                Name = "Private Lesson Rejected",
                Subject = "Private Lesson Request - Alternative Times Available",
                Body = @"<html>
<head>
    <style>
        body { font-family: Arial, sans-serif; line-height: 1.6; color: #333; }
        .container { max-width: 600px; margin: 0 auto; padding: 20px; }
        .header { background: linear-gradient(135deg, #f093fb 0%, #f5576c 100%); 
                  color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }
        .content { background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }
        .reason-box { background: #fff3cd; padding: 15px; margin: 20px 0; 
                      border-left: 4px solid #ffc107; border-radius: 5px; }
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>About Your Private Lesson Request</h1>
        </div>
        <div class='content'>
            <p>Hi {{StudentName}},</p>
            <p>Thank you for your interest in a private lesson with {{InstructorName}}.</p>
            <p>Unfortunately, the time you requested ({{RequestedDate}}) is not available.</p>
            <div class='reason-box'>
                <strong>Reason:</strong> {{Reason}}
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
                <p>Sumter Martial Arts<br>We're here to help! 🥋</p>
            </div>
        </div>
    </div>
</body>
</html>",
                Description = "Sent when a private lesson request is rejected. Variables: {{StudentName}}, {{InstructorName}}, {{RequestedDate}}, {{Reason}}",
                IsActive = true,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },

            // 9. Private Lesson Admin Notification
            new
            {
                Id = 9,
                TemplateKey = "PrivateLessonAdminNotification",
                Name = "Private Lesson Admin Notification",
                Subject = "🔔 New Private Lesson Request - {{StudentName}}",
                Body = @"<html>
<head>
    <style>
        body { font-family: Arial, sans-serif; line-height: 1.6; color: #333; }
        .container { max-width: 600px; margin: 0 auto; padding: 20px; }
        .header { background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); 
                  color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }
        .content { background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }
        .request-box { background: white; padding: 20px; margin: 20px 0; 
                       border: 2px solid #667eea; border-radius: 5px; }
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
                <p><strong>Student:</strong> {{StudentName}}</p>
                <p><strong>Requested Instructor:</strong> {{InstructorName}}</p>
                <p><strong>Requested Date:</strong> {{RequestedDate}}</p>
            </div>
            <p><strong>Action Required:</strong> Review and approve/reject this request in the admin panel.</p>
            <div style='text-align: center; margin-top: 30px; color: #666; font-size: 14px;'>
                <p>Sumter Martial Arts Admin Portal</p>
            </div>
        </div>
    </div>
</body>
</html>",
                Description = "Sent to admin when a new private lesson request is submitted. Variables: {{StudentName}}, {{InstructorName}}, {{RequestedDate}}",
                IsActive = true,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },

            // 10. Student Login Credentials
            new
            {
                Id = 10,
                TemplateKey = "StudentLoginCredentials",
                Name = "Student Login Credentials",
                Subject = "Your Student Portal Login Credentials 🔐",
                Body = @"<html>
<head>
    <style>
        body { font-family: Arial, sans-serif; line-height: 1.6; color: #333; }
        .container { max-width: 600px; margin: 0 auto; padding: 20px; }
        .header { background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); 
                  color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }
        .content { background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }
        .credentials-box { background: white; padding: 20px; margin: 20px 0; 
                           border: 2px solid #667eea; border-radius: 5px; }
        .password { font-family: monospace; font-size: 18px; color: #667eea; font-weight: bold; }
        .warning { background: #fff3cd; padding: 15px; margin: 20px 0; 
                   border-left: 4px solid #ffc107; border-radius: 5px; }
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Welcome to Your Student Portal!</h1>
        </div>
        <div class='content'>
            <p>Hi {{StudentName}},</p>
            <p>Your student portal account has been created! You can now log in to view your progress, 
               test history, program enrollments, and more.</p>
            <div class='credentials-box'>
                <p><strong>UserName:</strong> {{UserName}}</p>
                <p><strong>Temporary Password:</strong> <span class='password'>{{TemporaryPassword}}</span></p>
            </div>
            <div class='warning'>
                <strong>⚠️ Important Security Notice:</strong><br>
                This is a temporary password. For your security, you will be required to change it 
                after your first login.
            </div>
            <h3>What You Can Do in the Portal:</h3>
            <ul>
                <li>View your current rank and program enrollments</li>
                <li>Check your attendance records</li>
                <li>See your complete test history</li>
                <li>Update your contact information</li>
                <li>Read instructor notes and feedback</li>
            </ul>
            <h3>Getting Started:</h3>
            <ol>
                <li>Visit the student portal login page</li>
                <li>Enter your username and temporary password</li>
                <li>Follow the prompts to create your new password</li>
                <li>Explore your dashboard!</li>
            </ol>
            <p><strong>Having trouble logging in?</strong> Contact us and we'll be happy to help!</p>
            <p>We're excited to have you use the portal to track your martial arts journey!</p>
            <div style='text-align: center; margin-top: 30px; color: #666; font-size: 14px;'>
                <p>Sumter Martial Arts<br>Train Smart, Track Progress 🥋</p>
            </div>
        </div>
    </div>
</body>
</html>",
                Description = "Sent when a student account is created. Variables: {{StudentName}}, {{UserName}}, {{TemporaryPassword}}",
                IsActive = true,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },

            // 11. Instructor Login Credentials
            new
            {
                Id = 11,
                TemplateKey = "InstructorLoginCredentials",
                Name = "Instructor Login Credentials",
                Subject = "Your Instructor Portal Login Credentials 🔐",
                Body = @"<html>
<head>
    <style>
        body { font-family: Arial, sans-serif; line-height: 1.6; color: #333; }
        .container { max-width: 600px; margin: 0 auto; padding: 20px; }
        .header { background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); 
                  color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }
        .content { background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }
        .credentials-box { background: white; padding: 20px; margin: 20px 0; 
                           border: 2px solid #667eea; border-radius: 5px; }
        .password { font-family: monospace; font-size: 18px; color: #667eea; font-weight: bold; }
        .warning { background: #fff3cd; padding: 15px; margin: 20px 0; 
                   border-left: 4px solid #ffc107; border-radius: 5px; }
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Welcome to Your Instructor Portal!</h1>
        </div>
        <div class='content'>
            <p>Hi {{InstructorName}},</p>
            <p>Your instructor portal account has been created! You can now log in to manage your students, 
               record test results, track attendance, and more.</p>
            <div class='credentials-box'>
                <p><strong>UserName:</strong> {{UserName}}</p>
                <p><strong>Temporary Password:</strong> <span class='password'>{{TemporaryPassword}}</span></p>
            </div>
            <div class='warning'>
                <strong>⚠️ Important Security Notice:</strong><br>
                This is a temporary password. For your security, you will be required to change it 
                after your first login.
            </div>
            <h3>What You Can Do in the Portal:</h3>
            <ul>
                <li>View students in programs you teach</li>
                <li>Record test results and promotions</li>
                <li>Track student attendance</li>
                <li>Add instructor notes and feedback</li>
                <li>Manage your teaching schedule</li>
                <li>View and respond to private lesson requests</li>
            </ul>
            <h3>Getting Started:</h3>
            <ol>
                <li>Visit the instructor portal login page</li>
                <li>Enter your username and temporary password</li>
                <li>Follow the prompts to create your new password</li>
                <li>Start managing your students!</li>
            </ol>
            <p><strong>Having trouble logging in?</strong> Contact us and we'll be happy to help!</p>
            <div style='text-align: center; margin-top: 30px; color: #666; font-size: 14px;'>
                <p>Sumter Martial Arts<br>Empowering Instructors 🥋</p>
            </div>
        </div>
    </div>
</body>
</html>",
                Description = "Sent when an instructor account is created. Variables: {{InstructorName}}, {{UserName}}, {{TemporaryPassword}}",
                IsActive = true,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}