using System;
using System.Threading;
using Aliyun.MNS.Model;
using System.Collections.Generic;

namespace Aliyun.MNS.Sample
{
    /// <summary>
    /// Samples for all supported sync operations of MNS Queue.
    /// </summary>
    public class SyncOperationSample
    {
        #region Private Properties
        private const string _accessKeyId = "<your access key id>";
        private const string _secretAccessKey = "<your secret access key>";
        private const string _endpoint = "<valid endpoint>";
		private const string _stsToken = null;

        private const string _queueName = "myqueue";
        private const string _queueNamePrefix = "my";
        private const int _receiveTimes = 1;
        private const int _receiveInterval = 2;
        private const int batchSize = 6;
        private static string _receiptHandle;

        #endregion

        #region Main Routine

        static void Main(string[] args)
        {
            #region Queue Releated Test Cases

			IMNS client = new Aliyun.MNS.MNSClient(_accessKeyId, _secretAccessKey, _endpoint, _stsToken);

            try
            {
                var resp = client.GetAccountAttributes();
                Console.WriteLine("GetAccountAttributes success");
                Console.WriteLine(resp.Attributes.LoggingBucket);
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetAccountAttributes failed, exception info: " + ex.Message);
            }

            try
            {
                AccountAttributes aa = new AccountAttributes() { LoggingBucket = "Test" };
                var resp = client.SetAccountAttributes(aa);
                Console.WriteLine("SetAccountAttributes success");
            }
            catch (Exception ex)
            {
                Console.WriteLine("SetAccountAttributes failed, exception info: " + ex.Message);
            }

            try
            {
                var resp = client.GetAccountAttributes();
                Console.WriteLine("GetAccountAttributes success");
                Console.WriteLine(resp.Attributes.LoggingBucket);
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetAccountAttributes failed, exception info: " + ex.Message);
            }

            /* 1.1. Create queue */
            var createQueueRequest = new CreateQueueRequest
            {
                QueueName = _queueName,
                Attributes =
                {
                    DelaySeconds = 10,
                    VisibilityTimeout = 30,
                    MaximumMessageSize = 40960,
                    MessageRetentionPeriod = 345600,
                    PollingWaitSeconds = 15
                }
            };

            try
            {
                var queue = client.CreateQueue(createQueueRequest);
                Console.WriteLine("Create queue successfully, queue name: {0}", queue.QueueName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Create queue failed, exception info: " + ex.Message);
            }

            /* 2.1. Get queue attributes */
            try
            {
                var nativeQueue = client.GetNativeQueue(_queueName);
                var getQueueAttributesResponse = nativeQueue.GetAttributes();
                Console.WriteLine("Get queue attributes successfully, status code: {0}", getQueueAttributesResponse.HttpStatusCode);
                Console.WriteLine("----------------------------------------------------");
                Console.WriteLine("QueueName: {0}", getQueueAttributesResponse.Attributes.QueueName);
                Console.WriteLine("CreateTime: {0}", getQueueAttributesResponse.Attributes.CreateTime);
                Console.WriteLine("LastModifyTime: {0}", getQueueAttributesResponse.Attributes.LastModifyTime);
                Console.WriteLine("VisibilityTimeout: {0}", getQueueAttributesResponse.Attributes.VisibilityTimeout);
                Console.WriteLine("MaximumMessageSize: {0}", getQueueAttributesResponse.Attributes.MaximumMessageSize);
                Console.WriteLine("MessageRetentionPeriod: {0}", getQueueAttributesResponse.Attributes.MessageRetentionPeriod);
                Console.WriteLine("DelaySeconds: {0}", getQueueAttributesResponse.Attributes.DelaySeconds);
                Console.WriteLine("PollingWaitSeconds: {0}", getQueueAttributesResponse.Attributes.PollingWaitSeconds);
                Console.WriteLine("InactiveMessages: {0}", getQueueAttributesResponse.Attributes.InactiveMessages);
                Console.WriteLine("ActiveMessages: {0}", getQueueAttributesResponse.Attributes.ActiveMessages);
                Console.WriteLine("DelayMessages: {0}", getQueueAttributesResponse.Attributes.DelayMessages);
                Console.WriteLine("LoggingEnabled: {0}", getQueueAttributesResponse.Attributes.LoggingEnabled);
                Console.WriteLine("----------------------------------------------------\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Get queue attributes failed, exception info: " + ex.Message);
            }

            /* 3. List queue */
            try
            {
                var nextMarker = string.Empty;
                ListQueueResponse listQueueResponse;
                do
                {
                    var listQueueRequest = new ListQueueRequest
                    {
                        QueueNamePrefix = _queueNamePrefix,
                        Marker = nextMarker,
                        MaxReturns = 5
                    };
                    
                    listQueueResponse = client.ListQueue(listQueueRequest);
                    foreach (var queueUrl in listQueueResponse.QueueUrls)
                    {
                        Console.WriteLine(queueUrl);
                    }

                    Console.WriteLine("\n----------------------------------------------------\n");
                    
                    if (listQueueResponse.IsSetNextMarker())
                    {
                        nextMarker = listQueueResponse.NextMarker;
                        Console.WriteLine("NextMarker: {0}", listQueueResponse.NextMarker);
                    }
                } while (listQueueResponse.IsSetNextMarker());
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("List queue failed, exception info: " + ex.Message);
            }

            /* 4. Set queue attributes */
            var setQueueAttributesRequest = new SetQueueAttributesRequest
            {
                Attributes =
                {
                    DelaySeconds = 0,
                    VisibilityTimeout = 15,
                    MaximumMessageSize = 10240,
                    PollingWaitSeconds = 10,
                    MessageRetentionPeriod = 50000
                }
            };
            try
            {
                var nativeQueue = client.GetNativeQueue(_queueName);
                var setQueueAttributesResponse = nativeQueue.SetAttributes(setQueueAttributesRequest);
                Console.WriteLine("Set queue attributes successfully, status code: {0}", setQueueAttributesResponse.HttpStatusCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Set queue attributes failed, exception info: " + ex.Message);
            }

            /* 2.2. Get queue attributes again */
            try
            {
                var nativeQueue = client.GetNativeQueue(_queueName);
                var getQueueAttributesResponse = nativeQueue.GetAttributes();
                Console.WriteLine("Get queue attributes successfully again, status code: {0}", getQueueAttributesResponse.HttpStatusCode);
                Console.WriteLine("----------------------------------------------------");
                Console.WriteLine("QueueName: {0}", getQueueAttributesResponse.Attributes.QueueName);
                Console.WriteLine("CreateTime: {0}", getQueueAttributesResponse.Attributes.CreateTime);
                Console.WriteLine("LastModifyTime: {0}", getQueueAttributesResponse.Attributes.LastModifyTime);
                Console.WriteLine("VisibilityTimeout: {0}", getQueueAttributesResponse.Attributes.VisibilityTimeout);
                Console.WriteLine("MaximumMessageSize: {0}", getQueueAttributesResponse.Attributes.MaximumMessageSize);
                Console.WriteLine("MessageRetentionPeriod: {0}", getQueueAttributesResponse.Attributes.MessageRetentionPeriod);
                Console.WriteLine("DelaySeconds: {0}", getQueueAttributesResponse.Attributes.DelaySeconds);
                Console.WriteLine("PollingWaitSeconds: {0}", getQueueAttributesResponse.Attributes.PollingWaitSeconds);
                Console.WriteLine("InactiveMessages: {0}", getQueueAttributesResponse.Attributes.InactiveMessages);
                Console.WriteLine("ActiveMessages: {0}", getQueueAttributesResponse.Attributes.ActiveMessages);
                Console.WriteLine("DelayMessages: {0}", getQueueAttributesResponse.Attributes.DelayMessages);
                Console.WriteLine("----------------------------------------------------\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Get queue attributes failed again, exception info: " + ex.Message);
            }

            /* 5.1. Delete queue */
            var deleteQueueRequest = new DeleteQueueRequest(_queueName);
            deleteQueueRequest.AddHeader("Accept", "IE6"); //Add extra request headers
            //deleteQueueRequest.AddParameter("param1", "value1"); //InvalidQueryString
            try
            {
                var deleteQueueResponse = client.DeleteQueue(deleteQueueRequest);
                Console.WriteLine("Delete queue successfully, status code: {0}", deleteQueueResponse.HttpStatusCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Delete queue failed, exception info: " + ex.Message);
            } 

            /* 1.2. Create queue again */
            try
            {
                var queue = client.CreateQueue(createQueueRequest);
                Console.WriteLine("Create queue successfully again, queue name: {0}", queue.QueueName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Create queue failed again, exception info: " + ex.Message);
            }

            #endregion

            #region Message Releated Test Cases

            /* 6. Receive message */
            try
            {
                var nativeQueue = client.GetNativeQueue(_queueName);
                for (int i = 0; i < _receiveTimes; i++)
                {
                    var receiveMessageResponse = nativeQueue.ReceiveMessage();
                    Console.WriteLine("Receive message successfully, status code: {0}", receiveMessageResponse.HttpStatusCode);
                    Console.WriteLine("----------------------------------------------------");
                    Message message = receiveMessageResponse.Message;
                    Console.WriteLine("MessageId: {0}", message.Id);
                    Console.WriteLine("ReceiptHandle: {0}", message.ReceiptHandle);
                    Console.WriteLine("MessageBody: {0}", message.Body);
                    Console.WriteLine("MessageBodyMD5: {0}", message.BodyMD5);
                    Console.WriteLine("EnqueueTime: {0}", message.EnqueueTime);
                    Console.WriteLine("NextVisibleTime: {0}", message.NextVisibleTime);
                    Console.WriteLine("FirstDequeueTime: {0}", message.FirstDequeueTime);
                    Console.WriteLine("DequeueCount: {0}", message.DequeueCount);
                    Console.WriteLine("Priority: {0}", message.Priority);
                    Console.WriteLine("----------------------------------------------------\n");

                    _receiptHandle = message.ReceiptHandle;

                    Thread.Sleep(_receiveInterval);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Receive message failed, exception info: " + ex.Message + ex.GetType().Name);
            }

            /* 7. Send message */
            try
            {
                var nativeQueue = client.GetNativeQueue(_queueName);
                //var sendMessageResponse = nativeQueue.SendMessage("Hello world!", 10, 4);
                var sendMessageRequest = new SendMessageRequest("阿里云<MessageBody>计算");
                sendMessageRequest.DelaySeconds = 2;
                var sendMessageResponse = nativeQueue.SendMessage(sendMessageRequest);
                Console.WriteLine("Send message successfully,{0}", 
                    sendMessageResponse.ToString());
                Thread.Sleep(2000);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Send message failed, exception info: " + ex.Message);
            }

            /* 8. Receive message */
            try
            {
                var nativeQueue = client.GetNativeQueue(_queueName);
                for (int i = 0; i < _receiveTimes; i++)
                {
                    var receiveMessageResponse = nativeQueue.ReceiveMessage(30);
                    Console.WriteLine("Receive message successfully, status code: {0}", receiveMessageResponse.HttpStatusCode);
                    Console.WriteLine("----------------------------------------------------");
                    Message message = receiveMessageResponse.Message;
                    Console.WriteLine("MessageId: {0}", message.Id);
                    Console.WriteLine("ReceiptHandle: {0}", message.ReceiptHandle);
                    Console.WriteLine("MessageBody: {0}", message.Body);
                    Console.WriteLine("MessageBodyMD5: {0}", message.BodyMD5);
                    Console.WriteLine("EnqueueTime: {0}", message.EnqueueTime);
                    Console.WriteLine("NextVisibleTime: {0}", message.NextVisibleTime);
                    Console.WriteLine("FirstDequeueTime: {0}", message.FirstDequeueTime);
                    Console.WriteLine("DequeueCount: {0}", message.DequeueCount);
                    Console.WriteLine("Priority: {0}", message.Priority);
                    Console.WriteLine("----------------------------------------------------\n");

                    _receiptHandle = message.ReceiptHandle;

                    Thread.Sleep(_receiveInterval);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Receive message failed, exception info: " + ex.Message);
            } 

            /* 9. Change message visibility */
            try
            {
                var nativeQueue = client.GetNativeQueue(_queueName);
                var changeMessageVisibilityRequest = new ChangeMessageVisibilityRequest
                {
                    ReceiptHandle = _receiptHandle,
                    VisibilityTimeout = 5
                };
                var changeMessageVisibilityResponse = nativeQueue.ChangeMessageVisibility(changeMessageVisibilityRequest);
                Console.WriteLine("Change message visibility successfully, ReceiptHandle: {0}, NextVisibleTime: {1}", 
                    changeMessageVisibilityResponse.ReceiptHandle, changeMessageVisibilityResponse.NextVisibleTime);
                _receiptHandle = changeMessageVisibilityResponse.ReceiptHandle;

                Thread.Sleep(6000);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Change message visibility failed, exception info: " + ex.Message);
            } 

            /* 10. Peek message */
           try
           {
               var nativeQueue = client.GetNativeQueue(_queueName);
               for (uint i = 0; i < _receiveTimes; i++)
               {
                   var peekMessageResponse = nativeQueue.PeekMessage();
                   Console.WriteLine("Peek message successfully, status code: {0}", peekMessageResponse.HttpStatusCode);
                   Console.WriteLine("----------------------------------------------------");
                   Message message = peekMessageResponse.Message;
                   Console.WriteLine("MessageId: {0}", message.Id);
                   Console.WriteLine("MessageBody: {0}", message.Body);
                   Console.WriteLine("MessageBodyMD5: {0}", message.BodyMD5);
                   Console.WriteLine("EnqueueTime: {0}", message.EnqueueTime);
                   Console.WriteLine("FirstDequeueTime: {0}", message.FirstDequeueTime);
                   Console.WriteLine("DequeueCount: {0}", message.DequeueCount);
                   Console.WriteLine("Priority: {0}", message.Priority);
                   Console.WriteLine("----------------------------------------------------\n");

                   Thread.Sleep(_receiveInterval);
               }
           }
           catch (Exception ex)
           {
               Console.WriteLine("Peek message failed, exception info: " + ex.Message);
           }

            /* 11. Delete message */
            var deletedReceiptHandle = _receiptHandle;
            try
            {
                var nativeQueue = client.GetNativeQueue(_queueName);
                var receiveMessageResponse = nativeQueue.ReceiveMessage();
                _receiptHandle = receiveMessageResponse.Message.ReceiptHandle;
                var deleteMessageResponse = nativeQueue.DeleteMessage(_receiptHandle);
                Console.WriteLine("Delete message successfully, status code: {0}", deleteMessageResponse.HttpStatusCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Delete message failed, exception info: " + ex.Message);
            }

            #endregion

            #region Batch Message Related Operations
            /* 12. Batch Send message */
            try
            {
                var nativeQueue = client.GetNativeQueue(_queueName);
                //var sendMessageResponse = nativeQueue.SendMessage("Hello world!", 10, 4);

                List<SendMessageRequest> requests = new List<SendMessageRequest>();
                requests.Add(new SendMessageRequest("阿里云计算 Priority1", 0, 1));
                for (int i = 0; i < batchSize; i++)
                {
                    requests.Add(new SendMessageRequest("阿里云计算" + i.ToString()));
                }
                BatchSendMessageRequest batchSendRequest = new BatchSendMessageRequest()
                {
                    Requests = requests
                };
                var sendMessageResponse = nativeQueue.BatchSendMessage(batchSendRequest);
                Console.WriteLine("Batch send message successful! messages count {0}", sendMessageResponse.Responses.Count);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Batch send message failed, exception info: " + ex.Message);
                if (ex is BatchSendFailException)
                {
                    var errorItems = ((BatchSendFailException)ex).ErrorItems;
                    foreach (var errorItem in errorItems)
                    {
                        Console.WriteLine(errorItem.ToString());
                    }
                    var sentItems = ((BatchSendFailException)ex).SentMessageResponses;
                    foreach (var sentItem in sentItems)
                    {
                        Console.WriteLine(sentItem.ToString());
                    }
                }
            }

            Thread.Sleep(12000);

            /* 13. Batch Peek message */
            try
            {
                var nativeQueue = client.GetNativeQueue(_queueName);
                var batchPeekMessageResponse = nativeQueue.BatchPeekMessage(batchSize + 1);
                Console.WriteLine("Batch peek message successfully, status code: {0}, messages count {1}",
                    batchPeekMessageResponse.HttpStatusCode, batchPeekMessageResponse.Messages.Count);
                /*Console.WriteLine("----------------------------------------------------");
                foreach (var message in batchPeekMessageResponse.Messages)
                {
                    Console.WriteLine("MessageId: {0}", message.Id);
                    Console.WriteLine("MessageBody: {0}", message.Body);
                    Console.WriteLine("MessageBodyMD5: {0}", message.BodyMD5);
                    Console.WriteLine("EnqueueTime: {0}", message.EnqueueTime);
                    Console.WriteLine("FirstDequeueTime: {0}", message.FirstDequeueTime);
                    Console.WriteLine("DequeueCount: {0}", message.DequeueCount);
                    Console.WriteLine("Priority: {0}", message.Priority);
                    Console.WriteLine("----------------------------------------------------\n");
                }*/
            }
            catch (Exception ex)
            {
                Console.WriteLine("Batch peek message failed, exception info: " + ex.Message);
            }

            /* 14. Batch Receive message */
            BatchReceiveMessageResponse batchReceiveMessageResponse = null;
            try
            {
                var nativeQueue = client.GetNativeQueue(_queueName);
                batchReceiveMessageResponse = nativeQueue.BatchReceiveMessage(batchSize + 1, 3);
                Console.WriteLine("Batch receive message successfully, status code: {0}, messages count {1}", 
                    batchReceiveMessageResponse.HttpStatusCode, batchReceiveMessageResponse.Messages.Count);
                Console.WriteLine("----------------------------------------------------");
                foreach (var message in batchReceiveMessageResponse.Messages)
                {
                    Console.WriteLine("MessageId: {0}", message.Id);
                    Console.WriteLine("ReceiptHandle: {0}", message.ReceiptHandle);
                    Console.WriteLine("MessageBody: {0}", message.Body);
                    Console.WriteLine("MessageBodyMD5: {0}", message.BodyMD5);
                    Console.WriteLine("EnqueueTime: {0}", message.EnqueueTime);
                    Console.WriteLine("NextVisibleTime: {0}", message.NextVisibleTime);
                    Console.WriteLine("FirstDequeueTime: {0}", message.FirstDequeueTime);
                    Console.WriteLine("DequeueCount: {0}", message.DequeueCount);
                    Console.WriteLine("Priority: {0}", message.Priority);
                    Console.WriteLine("----------------------------------------------------\n");

                    _receiptHandle = message.ReceiptHandle;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Batch receive message failed, exception info: " + ex.Message);
            }

            /* 15. Batch Delete message */
            if (batchReceiveMessageResponse != null && batchReceiveMessageResponse.Messages.Count > 0)
            {
                try
                {
                    var nativeQueue = client.GetNativeQueue(_queueName);
                    List<string> receiptHandles = new List<string>();
                    foreach (var message in batchReceiveMessageResponse.Messages)
                    {
                        receiptHandles.Add(message.ReceiptHandle);
                    }
                    receiptHandles.Add(deletedReceiptHandle);
                    var batchDeleteMessageRequest = new BatchDeleteMessageRequest()
                    {
                        ReceiptHandles = receiptHandles
                    };
                    var batchDeleteMessageResponse = nativeQueue.BatchDeleteMessage(batchDeleteMessageRequest);
                    Console.WriteLine("Batch delete message successfully, status code: {0}", batchDeleteMessageResponse.HttpStatusCode);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Batch delete message failed, exception info: " + ex.Message);
                    if (ex is BatchDeleteFailException)
                    {
                        var errorItems = ((BatchDeleteFailException)ex).ErrorItems;
                        foreach (var errorItem in errorItems)
                        {
                            Console.WriteLine(errorItem.ToString());
                        }
                    }
                }
            }
            #endregion

            #region Clean Generated Queue

            /* 5.2. Delete queue again */
            try
            {
                var deleteQueueResponse = client.DeleteQueue(deleteQueueRequest);
                Console.WriteLine("Delete queue successfully again, status code: {0}", deleteQueueResponse.HttpStatusCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Delete queue failed again, exception info: " + ex.Message);
            }

            #endregion

            Console.ReadKey();
        }

        #endregion
    }
}
