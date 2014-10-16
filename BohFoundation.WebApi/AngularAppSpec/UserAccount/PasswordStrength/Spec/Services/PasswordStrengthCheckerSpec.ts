module BohFoundation.UserAccount.PasswordStrength.Spec.Services {
    describe('PasswordStrengthChecker', () => {

        var scriptjsService, result, passwordStrengthChecker, password;

        beforeEach(() => {
            scriptjsService = sinon.stub({
                getZxcvbn: () => { },
                zxcvbnLoaded: () => { },
                zxcvbnScore: () => { }
            });

            passwordStrengthChecker = new PasswordStrength.Services.PasswordStrengthChecker(scriptjsService);
        });

        it('constructor should call getZxcvbn.', () => {
            sinon.assert.calledOnce(scriptjsService.getZxcvbn);
        });

        describe('check with null password', () => {

            beforeEach(() => {
                password = null;
                checkPassword();
            });

            it('should not call anything if the password is null.', () => {
                sinon.assert.notCalled(scriptjsService.zxcvbnLoaded);
            });

            it('should return null.', () => {
                expect(result).toBeNull();
            });
        });

        describe('check with not null password', () => {

            var message0 = 'Bad';
            var style0 = 'danger';

            beforeEach(() => {
                password = 'password';
                scriptjsService.zxcvbnLoaded.returns(true);
            });

            it('should not call for a score if zxcvbnLoaded is false.', () => {
                scriptjsService.zxcvbnLoaded.returns(false);
                checkPassword();
                sinon.assert.notCalled(scriptjsService.zxcvbnScore);
            });

            it('should call for a score if zxcvbnLoaded is true.', () => {
                checkPassword();
                sinon.assert.calledOnce(scriptjsService.zxcvbnScore);
            });

            it('should return the right object when zxcvbn is not loaded.', () => {
                scriptjsService.zxcvbnLoaded.returns(false);
                checkPassword();
                resultAsserts(style0, 0, message0);
            });

            describe('zxcvbn stuff goes right', () => {

                var message1 = 'Not Good';
                var style1 = 'danger';
                var message2 = 'OK';
                var style2 = 'warning';
                var message3 = 'Good';
                var style3 = 'primary';
                var message4 = 'Great';
                var style4 = 'success';
                var messageDefault = 'Danger';
                var styleDefault = 'danger';

                it('should return right object 0 score.', () => {
                    scriptjsService.zxcvbnScore.returns(0);
                    checkPassword();
                    resultAsserts(style0, 0, message0);
                });

                it('should return the right object 1 score.', () => {
                    scriptjsService.zxcvbnScore.returns(1);
                    checkPassword();
                    resultAsserts(style1, 1, message1);
                });

                it('should return the right object 2 score.', () => {
                    scriptjsService.zxcvbnScore.returns(2);
                    checkPassword();
                    resultAsserts(style2, 2, message2);
                });

                it('should return the right object 3 score.', () => {
                    scriptjsService.zxcvbnScore.returns(3);
                    checkPassword();
                    resultAsserts(style3, 3, message3);
                });

                it('should return the right object 4 score.', () => {
                    scriptjsService.zxcvbnScore.returns(4);
                    checkPassword();
                    resultAsserts(style4, 4, message4);
                });

                it('should return the right object default score.', () => {
                    scriptjsService.zxcvbnScore.returns(1238998);
                    checkPassword();
                    resultAsserts(styleDefault, 1238998, messageDefault);
                });
            });;

            function resultAsserts(style, score, message) {
                expect(result.style).toBe(style);
                expect(result.score).toBe(score);
                expect(result.message).toBe(message);
            }
        });

        function checkPassword() {
            result = passwordStrengthChecker.check(password);
        }
    });
} 