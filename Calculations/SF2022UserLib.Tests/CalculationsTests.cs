using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF2022UserLib.Tests
{
    [TestClass]
    public class CalculationsTests
    {
        [TestMethod]
        public void AvailablePeriods_NullStartTimes_ThrowsArgumentNullException()
        {
            var calculations = new Calculations();
            TimeSpan[] startTimes = null;
            int[] durations = { 30, 45 };

            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                calculations.AvailablePeriods(startTimes, durations, TimeSpan.FromHours(9), TimeSpan.FromHours(17), 60);
            });
        }

        [TestMethod]
        public void AvailablePeriods_NullDurations_ThrowsArgumentNullException()
        {
            var calculations = new Calculations();
            TimeSpan[] startTimes = { TimeSpan.FromHours(9), TimeSpan.FromHours(10) };
            int[] durations = null;

            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                calculations.AvailablePeriods(startTimes, durations, TimeSpan.FromHours(9), TimeSpan.FromHours(17), 60);
            });
        }

        [TestMethod]
        public void AvailablePeriods_ArraysLengthMismatch_ThrowsArrayMismatchException()
        {
            var calculations = new Calculations();
            TimeSpan[] startTimes = { TimeSpan.FromHours(9), TimeSpan.FromHours(10) };
            int[] durations = { 30 };

            Assert.ThrowsException<ArrayMismatchException>(() =>
            {
                calculations.AvailablePeriods(startTimes, durations, TimeSpan.FromHours(9), TimeSpan.FromHours(17), 60);
            });
        }

        [TestMethod]
        public void AvailablePeriods_NegativeConsultationTime_ThrowsArgumentException()
        {
            var calculations = new Calculations();
            TimeSpan[] startTimes = { TimeSpan.FromHours(9), TimeSpan.FromHours(10) };
            int[] durations = { 30, 45 };

            Assert.ThrowsException<ArgumentException>(() =>
            {
                calculations.AvailablePeriods(startTimes, durations, TimeSpan.FromHours(9), TimeSpan.FromHours(17), -30);
            });
        }


        [TestMethod]
        public void AvailablePeriods_EndBeforeStart_ThrowsArgumentOutOfRangeException()
        {
            var calculations = new Calculations();
            TimeSpan[] startTimes = { TimeSpan.FromHours(15) };
            int[] durations = { 30 };

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                calculations.AvailablePeriods(startTimes, durations, TimeSpan.FromHours(17), TimeSpan.FromHours(9), 60);
            });
        }

        [TestMethod]
        public void AvailablePeriods_OutsideWorkingHours_ThrowsArgumentOutOfRangeException()
        {
            var calculations = new Calculations();
            TimeSpan[] startTimes = { TimeSpan.FromHours(2) };
            int[] durations = { 30 };

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                calculations.AvailablePeriods(startTimes, durations, TimeSpan.FromHours(9), TimeSpan.FromHours(17), 60);
            });
        }

        [TestMethod]
        public void AvailablePeriods_NoExistingConsultations_ReturnsAvailablePeriodsInWorkingHours()
        {
            var calculations = new Calculations();
            TimeSpan[] startTimes = { };
            int[] durations = { };

            var result = calculations.AvailablePeriods(startTimes, durations, TimeSpan.FromHours(9), TimeSpan.FromHours(17), 60);

            Assert.AreEqual(result.Length, 8);
        }

        [TestMethod]
        public void AvailablePeriods_SingleConsultation_ReturnsCorrectAvailablePeriods()
        {
            var calculations = new Calculations();
            TimeSpan[] startTimes = { TimeSpan.FromHours(9) };
            int[] durations = { 30 };

            var result = calculations.AvailablePeriods(startTimes, durations, TimeSpan.FromHours(9), TimeSpan.FromHours(17), 60);

            Assert.AreEqual(result.Length, 7); // Ожидаемые месячные на основании одной консультации
        }

        [TestMethod]
        public void AvailablePeriods_MultipleConsultations_ReturnsCorrectAvailablePeriods()
        {
            var calculations = new Calculations();
            TimeSpan[] startTimes = { TimeSpan.FromHours(9), TimeSpan.FromHours(11), TimeSpan.FromHours(13) };
            int[] durations = { 30, 45, 60 };

            var result = calculations.AvailablePeriods(startTimes, durations, TimeSpan.FromHours(9), TimeSpan.FromHours(17), 60);

            Assert.AreEqual(result.Length, 5); // Ожидаемые периоды на основании нескольких консультаций
        }

        [TestMethod]
        public void Format_TimeSpan_ReturnsCorrectFormat()
        {
            // Arrange
            TimeSpan ts = new TimeSpan(3, 30, 0); // 3 часа 30 минут

            // Act
            string formattedTime = Calculations.Format(ts);

            // Assert
            Assert.AreEqual("03:30", formattedTime);
        }

    }
}
