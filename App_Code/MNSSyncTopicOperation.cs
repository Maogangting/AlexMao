using Aliyun.MNS.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Aliyun.MNS.Sample
{
    /// <summary>
    /// Samples for all supported sync operations of MNS.
    /// </summary>
    class SyncTopicOperation
    {
        #region Private Properties
        private const string _accessKeyId = "<your access key id>";
        private const string _secretAccessKey = "<your secret access key>";
        private const string _endpoint = "<valid endpoint>";

        const string _topicName = "TestCSharpTopic";
        const string _subscriptionName = "TestSub";

        #endregion

        #region main
        static void Main(string[] args)
        {
            #region Topic Releated Test Cases

            IMNS client = new Aliyun.MNS.MNSClient(_accessKeyId, _secretAccessKey, _endpoint);

            /* 1.1. Create queue */
            var createTopicRequest = new CreateTopicRequest
            {
                TopicName = _topicName
            };

            Topic topic = null;
            try
            {
                client.DeleteTopic(_topicName);
                topic = client.CreateTopic(createTopicRequest);
                Console.WriteLine("Create topic successfully, topic name: {0}", topic.TopicName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Create topic failed, exception info: " + ex.Message);
                return;
            }

            topic = client.GetNativeTopic(_topicName);
            try
            {
                ListTopicResponse res = client.ListTopic(null, null, 10);
                Console.WriteLine("List topic successfully, topic name: {0}", _topicName);
                foreach (String topicUrl in res.TopicUrls)
                {
                    Console.WriteLine(topicUrl);
                }
                if (res.NextMarker != null)
                {
                    Console.WriteLine("NextMarker: " + res.NextMarker);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Delete topic failed, exception info: " + ex.Message);
            }

            try
            {
                GetTopicAttributesResponse res = topic.GetAttributes();
                Console.WriteLine("GetTopicAttributes, topic name: {0}", _topicName);
                Console.WriteLine(res.Attributes.CreateTime);
                Console.WriteLine(res.Attributes.LastModifyTime);
                Console.WriteLine(res.Attributes.MaximumMessageSize);
                Console.WriteLine(res.Attributes.MessageRetentionPeriod);
                Console.WriteLine(res.Attributes.LoggingEnabled);
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetTopicAttributes failed, exception info: " + ex.Message);
            }

            try
            {
                TopicAttributes attributes = new TopicAttributes() { MaximumMessageSize = 2048};
                topic.SetAttributes(attributes);
                Console.WriteLine("SetTopicAttributes succeed, topic name: {0}", _topicName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("SetTopicAttributes failed, exception info: " + ex.Message + ex.GetType().Name);
            }

            try
            {
                SubscribeResponse res = topic.Subscribe(_subscriptionName, "http://XXXX");
                Console.WriteLine("Subscribe, subscriptionUrl: {0}", res.SubscriptionUrl);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Subscribe failed, exception info: " + ex.Message);
            }

            try
            {
                GetSubscriptionAttributeResponse res = topic.GetSubscriptionAttribute(_subscriptionName);
                Console.WriteLine("GetSubscriptionAttributeResponse, subs name: {0}", _subscriptionName);
                Console.WriteLine(res.Attributes.CreateTime);
                Console.WriteLine(res.Attributes.LastModifyTime);
                Console.WriteLine(res.Attributes.TopicName);
                Console.WriteLine(res.Attributes.TopicOwner);
                Console.WriteLine(res.Attributes.EndPoint);
                Console.WriteLine(res.Attributes.Strategy);
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetSubscriptionAttribute failed, exception info: " + ex.Message);
            }

            try
            {
                SubscriptionAttributes attributes = new SubscriptionAttributes() {
                    Strategy = SubscriptionAttributes.NotifyStrategy.EXPONENTIAL_DECAY_RETRY
                };
                topic.SetSubscriptionAttribute(_subscriptionName, attributes);
                Console.WriteLine("SetSubscriptionAttribute succeed, topic name: {0}", _topicName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("SetSubscriptionAttribute failed, exception info: " + ex.Message + ex.GetType().Name);
            }

            try
            {
                GetSubscriptionAttributeResponse res = topic.GetSubscriptionAttribute(_subscriptionName);
                Console.WriteLine("GetSubscriptionAttributeResponse, subs name: {0}", _subscriptionName);
                Console.WriteLine(res.Attributes.CreateTime);
                Console.WriteLine(res.Attributes.LastModifyTime);
                Console.WriteLine(res.Attributes.TopicName);
                Console.WriteLine(res.Attributes.TopicOwner);
                Console.WriteLine(res.Attributes.EndPoint);
                Console.WriteLine(res.Attributes.Strategy);
                Console.WriteLine(res.Attributes.ContentFormat);
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetSubscriptionAttribute failed, exception info: " + ex.Message);
            }

            try
            {
                ListSubscriptionResponse res = topic.ListSubscription("");
                Console.WriteLine("ListSubscription successfully, topic name: {0}", _topicName);
                foreach (String subscriptionUrl in res.SubscriptionUrls)
                {
                    Console.WriteLine(subscriptionUrl);
                }
                if (res.NextMarker != null)
                {
                    Console.WriteLine("NextMarker: " + res.NextMarker);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ListSubscription failed, exception info: " + ex.Message);
            }

            try
            {
                var response = topic.PublishMessage("message here </asdas\">");
                Console.WriteLine("PublishMessage succeed! " + response.MessageId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("PublishMessage failed, exception info: " + ex.Message);
                Console.WriteLine(ex.StackTrace);
            }

            try
            {
                topic.Unsubscribe(_subscriptionName);
                Console.WriteLine("Unsubscribe succeed!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Subscribe failed, exception info: " + ex.Message);
                Console.WriteLine(ex.StackTrace);
            }

            // PUBLISH MESSAGE for SMS
            try
            {
                var res = topic.Subscribe(_subscriptionName + "batchsms", topic.GenerateBatchSmsEndpoint());
                Console.WriteLine(res.SubscriptionUrl);

                PublishMessageRequest request = new PublishMessageRequest();
                MessageAttributes messageAttributes = new MessageAttributes();
                BatchSmsAttributes batchSmsAttributes = new BatchSmsAttributes();
                batchSmsAttributes.FreeSignName = "陈舟锋";
                batchSmsAttributes.TemplateCode = "SMS_15535414";
                Dictionary<string, string> param = new Dictionary<string, string>();
                param.Add("name", "CSharpBatch");
                batchSmsAttributes.AddReceiver("13735576932", param);

                messageAttributes.BatchSmsAttributes = batchSmsAttributes;
                request.MessageAttributes = messageAttributes;
                request.MessageBody = "</asdas\">";
                PublishMessageResponse resp = topic.PublishMessage(request);

                Console.WriteLine(resp.MessageId);

                // check sms
                System.Threading.Thread.Sleep(3000);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Subscribe failed, exception info: " + ex.Message);
            }

            // PUBLISH MESSAGE for SMS
            try
            {
                var res = topic.Subscribe(_subscriptionName + "singlesms", topic.GenerateSmsEndpoint());
                Console.WriteLine(res.SubscriptionUrl);

                PublishMessageRequest request = new PublishMessageRequest();
                MessageAttributes messageAttributes = new MessageAttributes();
                SmsAttributes smsAttributes = new SmsAttributes();
                smsAttributes.FreeSignName = "陈舟锋";
                smsAttributes.TemplateCode = "SMS_15535414";
                Dictionary<string, string> param = new Dictionary<string, string>();
                param.Add("name", "CSharpSingle");
                smsAttributes.Receiver = "13735576932";
                smsAttributes.SmsParams = param;

                messageAttributes.SmsAttributes = smsAttributes;
                request.MessageAttributes = messageAttributes;
                request.MessageBody = "</asdas\">";
                PublishMessageResponse resp = topic.PublishMessage(request);

                Console.WriteLine(resp.MessageId);

                // check sms
                System.Threading.Thread.Sleep(3000);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Subscribe failed, exception info: " + ex.Message);
            }

            // PUBLISH MESSAGE TO QUEUE AND MAIL
            string queueName = "TestQueueNameHere";
            try
            {
                var queue = client.CreateQueue(queueName);

                var res = topic.Subscribe(_subscriptionName, topic.GenerateMailEndpoint("liji.canglj@alibaba-inc.com"));
                // res = topic.Subscribe(_subscriptionName + "2", topic.GenerateQueueEndpoint(queueName));
                res = topic.Subscribe(new SubscribeRequest(_subscriptionName + "2", topic.GenerateQueueEndpoint(queueName), "TAG", SubscriptionAttributes.NotifyStrategy.BACKOFF_RETRY, SubscriptionAttributes.NotifyContentFormat.JSON));

                PublishMessageRequest request = new PublishMessageRequest();
                MessageAttributes messageAttributes = new MessageAttributes();
                MailAttributes mailAttributes = new MailAttributes();
                mailAttributes.AccountName = "mnspublic@mns.css9.net";
                mailAttributes.Subject = "TestMail C#";
                mailAttributes.IsHtml = false;
                mailAttributes.ReplyToAddress = false;
                mailAttributes.AddressType = 0;
                messageAttributes.MailAttributes = mailAttributes;
                request.MessageAttributes = messageAttributes;
                request.MessageTag = "TAG";
                request.MessageBody = "message here2222 </asdas\">";
                topic.PublishMessage(request);

                var resp = queue.ReceiveMessage(30);
                Console.WriteLine(resp.Message.Body);

                // check mailbox
                System.Threading.Thread.Sleep(3000);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Subscribe failed, exception info: " + ex.Message);
            }

            try
            {
                client.DeleteQueue(queueName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Delete queue failed, exception info: " + ex.Message);
            }

            try
            {
                client.DeleteTopic(_topicName);
                Console.WriteLine("Delete topic successfully, topic name: {0}", _topicName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Delete topic failed, exception info: " + ex.Message);
            }
        }
        #endregion
    }

    #endregion
}
