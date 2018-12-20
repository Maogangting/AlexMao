using System;
using System.Threading;
using Aliyun.MNS.Model;
using System.Collections.Generic;

namespace Aliyun.MNS.Sample
{
    /// <summary>
    /// Samples for all supported async operations of MNS.
    /// </summary>
    public class AsyncOperationSample
    {
        #region Private Properties

        private const string _accessKeyId = "<your access key id>";
        private const string _secretAccessKey = "<your secret access key>";
        private const string _endpoint = "<valid endpoint>";

        private static IMNS _client = new MNSClient(_accessKeyId, _secretAccessKey, _endpoint);

        private const string _queueName = "myqueue2";
        private const string _queueNamePrefix = "xqueue";
        private const int _receiveTimes = 1;
        private const int _receiveInterval = 2;
        private const uint _batchSize = 6;
        private static string _receiptHandle;
        private static string _nextMarker = string.Empty;
        private static AutoResetEvent _autoSetEvent = new AutoResetEvent(false);

        private static BatchReceiveMessageResponse _batchReceiveMessageResponse = null;

        #endregion

        #region Queue Releated Callback Methods

        static void CreateQueueCallback(IAsyncResult ar)
        {
            try
            {
                var queue = _client.EndCreateQueue(ar);
                Console.WriteLine("Async Create queue successfully, queue name: {0}", queue.QueueName);
                
                _autoSetEvent.Set();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Async Create queue failed, exception info: " + ex.Message);
            } 
        }

        static void DeleteQueueCallback(IAsyncResult ar)
        {
            try
            {
                var response = _client.EndDeleteQueue(ar);
                Console.WriteLine("Async Delete queue {0} successfully, status code: {1}", ar.AsyncState as string, response.HttpStatusCode);
                
                _autoSetEvent.Set();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Async Delete queue failed, exception info: " + ex.Message);
            } 
        }

        static void ListQueueCallback(IAsyncResult ar)
        {
            try
            {
                var response = _client.EndListQueue(ar);
                foreach (var queueUrl in response.QueueUrls)
                {
                    Console.WriteLine(queueUrl);
                }

                Console.WriteLine("\n----------------------------------------------------\n");

                if (response.IsSetNextMarker())
                {
                    _nextMarker = response.NextMarker;
                    Console.WriteLine("NextMarker: {0}", response.NextMarker);
                }
                else
                {
                    _nextMarker = string.Empty;
                }

                _autoSetEvent.Set();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Async List queue failed, exception info: " + ex.Message);
            }
        }

        static void GetQueueAttributesCallback(IAsyncResult ar)
        {
            try
            {
                var nativeQueue = (Queue)ar.AsyncState;
                var getQueueAttributesResponse = nativeQueue.EndGetAttributes(ar);
                Console.WriteLine("Async Get queue attributes successfully, status code: {0}", getQueueAttributesResponse.HttpStatusCode);
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

                _autoSetEvent.Set();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Async Get queue attributes failed, exception info: " + ex.Message);
            }
        }

        static void SetQueueAttributesCallback(IAsyncResult ar)
        {
            try
            {
                var nativeQueue = (Queue)ar.AsyncState;
                var setQueueAttributesResponse = nativeQueue.EndSetAttributes(ar);
                Console.WriteLine("Async Set queue attributes successfully, status code: {0}", setQueueAttributesResponse.HttpStatusCode);

                _autoSetEvent.Set();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Async Set queue attributes failed, exception info: " + ex.Message);
            }
        }

        #endregion

        #region Message Releated Callback Methods

        static void BatchSendMessageCallback(IAsyncResult ar)
        {
            try
            {
                var nativeQueue = (Queue)ar.AsyncState;
                var response = nativeQueue.EndBatchSendMessage(ar);
                Console.WriteLine("Async Batch send message successfully, messages count {0}", response.Responses.Count);

                _autoSetEvent.Set();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Async Batch send message failed, exception info: " + ex.Message + ex.GetType().Name);
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
                    _autoSetEvent.Set();
                }
            }
        }

        static void SendMessageCallback(IAsyncResult ar)
        {
            try
            {
                var nativeQueue = (Queue)ar.AsyncState;
                var response = nativeQueue.EndSendMessage(ar);
                Console.WriteLine("Async Send message successfully, status code: {0}, MessageBodyMD5: {1}",
                    response.MessageId, response.MessageBodyMD5);

                _autoSetEvent.Set();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Async Send message failed, exception info: " + ex.Message);
            }
        }

        static void ReceiveMessageCallback(IAsyncResult ar)
        {
            try
            {
                var nativeQueue = (Queue)ar.AsyncState;
                var response = nativeQueue.EndReceiveMessage(ar);
                Console.WriteLine("Async Receive message successfully, status code: {0}", response.HttpStatusCode);
                Console.WriteLine("----------------------------------------------------");
                var message = response.Message;
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

                _autoSetEvent.Set();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Async Receive message failed, exception info: " + ex.Message + ex.GetType().Name);
                _autoSetEvent.Set();
            }
        }

        static void BatchReceiveMessageCallback(IAsyncResult ar)
        {
            try
            {
                var nativeQueue = (Queue)ar.AsyncState;
                _batchReceiveMessageResponse = nativeQueue.EndBatchReceiveMessage(ar);
                Console.WriteLine("Async Batch receive message successfully, status code: {0}, messages count {1}",
                    _batchReceiveMessageResponse.HttpStatusCode, _batchReceiveMessageResponse.Messages.Count);
                Console.WriteLine("----------------------------------------------------");

                _autoSetEvent.Set();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Async Batch receive message failed, exception info: " + ex.Message);
            }
        }

        static void ChangeMessageVisibilityCallback(IAsyncResult ar)
        {
            try
            {
                var nativeQueue = (Queue) ar.AsyncState;
                var changeMessageVisibilityResponse = nativeQueue.EndChangeMessageVisibility(ar);
                Console.WriteLine("Async Change message visibility successfully, ReceiptHandle: {0}, NextVisibleTime: {1}",
                    changeMessageVisibilityResponse.ReceiptHandle, changeMessageVisibilityResponse.NextVisibleTime);
                
                _receiptHandle = changeMessageVisibilityResponse.ReceiptHandle;

                Thread.Sleep(6000);

                _autoSetEvent.Set();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Async Change message visibility failed, exception info: " + ex.Message);
            }
        }

        static void PeekMessageCallback(IAsyncResult ar)
        {
            try
            {
                var nativeQueue = (Queue) ar.AsyncState;
                var peekMessageResponse = nativeQueue.EndPeekMessage(ar);
                Console.WriteLine("Async Peek message successfully, status code: {0}", peekMessageResponse.HttpStatusCode);
                Console.WriteLine("----------------------------------------------------");
                var message = peekMessageResponse.Message;
                Console.WriteLine("MessageId: {0}", message.Id);
                Console.WriteLine("MessageBody: {0}", message.Body);
                Console.WriteLine("MessageBodyMD5: {0}", message.BodyMD5);
                Console.WriteLine("EnqueueTime: {0}", message.EnqueueTime);
                Console.WriteLine("FirstDequeueTime: {0}", message.FirstDequeueTime);
                Console.WriteLine("DequeueCount: {0}", message.DequeueCount);
                Console.WriteLine("Priority: {0}", message.Priority);
                Console.WriteLine("----------------------------------------------------\n");

                _autoSetEvent.Set();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Async Peek message failed, exception info: " + ex.Message);
            } 
        }

        static void BatchPeekMessageCallback(IAsyncResult ar)
        {
            try
            {
                var nativeQueue = (Queue)ar.AsyncState;
                var batchPeekMessageResponse = nativeQueue.EndBatchPeekMessage(ar);
                Console.WriteLine("Async Batch peek message successfully, status code: {0}, messages count {1}",
                batchPeekMessageResponse.HttpStatusCode, batchPeekMessageResponse.Messages.Count);

                _autoSetEvent.Set();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Async Batch peek message failed, exception info: " + ex.Message);
            }
        }

        static void DeleteMessageCallback(IAsyncResult ar)
        {
            try
            {
                var nativeQueue = (Queue) ar.AsyncState;
                var deleteMessageResponse = nativeQueue.EndDeleteMessage(ar);
                Console.WriteLine("Async Delete message successfully, status code: {0}", deleteMessageResponse.HttpStatusCode);

                _autoSetEvent.Set();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Async Delete message failed, exception info: " + ex.Message);
            }
        }

        static void BatchDeleteMessageCallback(IAsyncResult ar)
        {
            try
            {
                var nativeQueue = (Queue)ar.AsyncState;
                var batchDeleteMessageResponse = nativeQueue.EndBatchDeleteMessage(ar);
                Console.WriteLine("Async Batch delete message successfully, status code: {0}", batchDeleteMessageResponse.HttpStatusCode);

                _autoSetEvent.Set();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Async Batch delete message failed, exception info: " + ex.Message);
                if (ex is BatchDeleteFailException)
                {
                    var errorItems = ((BatchDeleteFailException)ex).ErrorItems;
                    foreach (var errorItem in errorItems)
                    {
                        Console.WriteLine(errorItem.ToString());
                    }
                    _autoSetEvent.Set();
                }
            }
        }

        #endregion

        #region Main Routine

        static void Main(string[] args)
        {
            #region Queue Releated Test Cases

            /* 1.1. Async create queue */
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
                _client.BeginCreateQueue(createQueueRequest, CreateQueueCallback, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Create queue failed, exception info: " + ex.Message);
            }

            _autoSetEvent.WaitOne();

            /* 2.1 Async get queue attributes */
            try
            {
                var nativeQueue = _client.GetNativeQueue(_queueName);
                var getQueueAttributesRequest = new GetQueueAttributesRequest();
                nativeQueue.BeginGetAttributes(getQueueAttributesRequest, GetQueueAttributesCallback, nativeQueue);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Get queue attributes failed, exception info: " + ex.Message);
            }

            _autoSetEvent.WaitOne();

            /* 3. Async list queue */
            try
            {
                do
                {
                    var listQueueRequest = new ListQueueRequest
                    {
                        QueueNamePrefix = _queueNamePrefix,
                        Marker = _nextMarker,
                        MaxReturns = 5
                    };
                    
                    _client.BeginListQueue(listQueueRequest, ListQueueCallback, null);
                    
                    _autoSetEvent.WaitOne();

                } while (_nextMarker != string.Empty);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("List queue failed, exception info: " + ex.Message);
            }

            /* 4. Async set queue attributes */
            var setQueueAttributesRequest = new SetQueueAttributesRequest
            {
                Attributes =
                {
                    DelaySeconds = 0,
                    VisibilityTimeout = 10,
                    MaximumMessageSize = 10240,
                    PollingWaitSeconds = 10,
                    MessageRetentionPeriod = 50000
                }
            };
            try
            {
                var nativeQueue = _client.GetNativeQueue(_queueName);
                nativeQueue.BeginSetAttributes(setQueueAttributesRequest, SetQueueAttributesCallback, nativeQueue);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Set queue attributes failed, exception info: " + ex.Message);
            }

            _autoSetEvent.WaitOne();

            /* 2.2 Async get queue attributes again */
            try
            {
                var nativeQueue = _client.GetNativeQueue(_queueName);
                var getQueueAttributesRequest = new GetQueueAttributesRequest();
                nativeQueue.BeginGetAttributes(getQueueAttributesRequest, GetQueueAttributesCallback, nativeQueue);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Get queue attributes failed again, exception info: " + ex.Message);
            }

            _autoSetEvent.WaitOne();

            /* 5.1. Async delete queue */
            var deleteQueueRequest = new DeleteQueueRequest(_queueName);
            deleteQueueRequest.AddHeader("Accept", "IE6");
            try
            {
                _client.BeginDeleteQueue(deleteQueueRequest, DeleteQueueCallback, _queueName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Delete queue failed, exception info: " + ex.Message);
            }

            _autoSetEvent.WaitOne();

            /* 1.2. Async create queue again */
            try
            {
                _client.BeginCreateQueue(createQueueRequest, CreateQueueCallback, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Create queue failed again, exception info: " + ex.Message);
            }

            _autoSetEvent.WaitOne();

            #endregion

            #region Messge Releated Test Cases

            /* 5. Async receive message */
            try
            {
                var nativeQueue = _client.GetNativeQueue(_queueName);
                for (int i = 0; i < _receiveTimes; i++)
                {
                    var receiveMessageRequest = new ReceiveMessageRequest();
                    nativeQueue.BeginReceiveMessage(receiveMessageRequest, ReceiveMessageCallback, nativeQueue);

                    _autoSetEvent.WaitOne();

                    Thread.Sleep(_receiveInterval);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Receive message failed, exception info: " + ex.Message);
            }

            /* 6. Async send message */
            try
            {
                var nativeQueue = _client.GetNativeQueue(_queueName);
                var sendMessageRequest = new SendMessageRequest("阿里云计算");
                nativeQueue.BeginSendMessage(sendMessageRequest, SendMessageCallback, nativeQueue);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Send message failed, exception info: " + ex.Message);
            }

            _autoSetEvent.WaitOne(); 

            /* 7. Async receive message */
            try
            {
                var nativeQueue = _client.GetNativeQueue(_queueName);
                for (int i = 0; i < _receiveTimes; i++)
                {
                    var receiveMessageRequest = new ReceiveMessageRequest();
                    nativeQueue.BeginReceiveMessage(receiveMessageRequest, ReceiveMessageCallback, nativeQueue);

                    _autoSetEvent.WaitOne();

                    Thread.Sleep(_receiveInterval);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Receive message failed, exception info: " + ex.Message);
            }

            /* 8. Async change message visibility */
            try
            {
                var nativeQueue = _client.GetNativeQueue(_queueName);
                var changeMessageVisibilityRequest = new ChangeMessageVisibilityRequest
                {
                    ReceiptHandle = _receiptHandle,
                    VisibilityTimeout = 5
                };
                nativeQueue.BeginChangeMessageVisibility(changeMessageVisibilityRequest, ChangeMessageVisibilityCallback, nativeQueue);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Change message visibility failed, exception info: " + ex.Message);
            }

            _autoSetEvent.WaitOne();

            /* 9. Async peek message */
           try
           {
               var nativeQueue = _client.GetNativeQueue(_queueName);
               var peekMessageRequest = new PeekMessageRequest();
               for (uint i = 0; i < _receiveTimes; i++)
               {
                   nativeQueue.BeginPeekMessage(peekMessageRequest, PeekMessageCallback, nativeQueue);

                   _autoSetEvent.WaitOne();

                   Thread.Sleep(_receiveInterval);
               }
           }
           catch (Exception ex)
           {
               Console.WriteLine("Peek message failed, exception info: " + ex.Message);
           }

           /* 10. Async delete message */
            try
            {
                var nativeQueue = _client.GetNativeQueue(_queueName);
                var receiveMessageResponse = nativeQueue.ReceiveMessage();
                _receiptHandle = receiveMessageResponse.Message.ReceiptHandle;
                var deleteMessageRequest = new DeleteMessageRequest(_receiptHandle);
                nativeQueue.BeginDeleteMessage(deleteMessageRequest, DeleteMessageCallback, nativeQueue);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Async BeginDeleteMessage failed, exception info: " + ex.Message);
            }

            _autoSetEvent.WaitOne();

            /* 11. Async batch send message */
            try
            {
                var nativeQueue = _client.GetNativeQueue(_queueName);
                List<SendMessageRequest> requests = new List<SendMessageRequest>();
                requests.Add(new SendMessageRequest("阿里云计算 Priority1", 0, 1));
                for (int i = 0; i < _batchSize; i++)
                {
                    requests.Add(new SendMessageRequest("阿里云计算" + i.ToString()));
                }
                BatchSendMessageRequest batchSendRequest = new BatchSendMessageRequest()
                {
                    Requests = requests
                };
                nativeQueue.BeginBatchSendMessage(batchSendRequest, BatchSendMessageCallback, nativeQueue);

                _autoSetEvent.WaitOne();
            }
            catch (Exception ex)
            {
                Console.WriteLine("BeginBatchSend message failed, exception info: " + ex.Message);
            }

            Thread.Sleep(12000);

            /* 12. Async batch peek message */
            try
            {
                var nativeQueue = _client.GetNativeQueue(_queueName);
                var batchPeekMessageRequest = new BatchPeekMessageRequest(_batchSize + 2);
                Console.WriteLine(batchPeekMessageRequest.BatchSize.ToString());
                nativeQueue.BeginBatchPeekMessage(batchPeekMessageRequest, BatchPeekMessageCallback, nativeQueue);

                _autoSetEvent.WaitOne();
            }
            catch (Exception ex)
            {
                Console.WriteLine("BeginBatchPeek message failed, exception info: " + ex.Message);
            }

            /* 13. Async batch receive message */
            try
            {
                var nativeQueue = _client.GetNativeQueue(_queueName);
                BatchReceiveMessageRequest batchReceiveMessageRequest = new BatchReceiveMessageRequest(_batchSize + 1, 3);
                var batchReceiveMessageResponse = nativeQueue.BeginBatchReceiveMessage(batchReceiveMessageRequest, BatchReceiveMessageCallback, nativeQueue);
                _autoSetEvent.WaitOne();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Batch receive message failed, exception info: " + ex.Message);
            }

            /* 14. Async batch delete message */
            if (_batchReceiveMessageResponse != null && _batchReceiveMessageResponse.Messages.Count > 0)
            {
                try
                {
                    var nativeQueue = _client.GetNativeQueue(_queueName);
                    List<string> receiptHandles = new List<string>();
                    foreach (var message in _batchReceiveMessageResponse.Messages)
                    {
                        receiptHandles.Add(message.ReceiptHandle);
                    }
                    var batchDeleteMessageRequest = new BatchDeleteMessageRequest()
                    {
                        ReceiptHandles = receiptHandles
                    };
                    var batchDeleteMessageResponse = nativeQueue.BeginBatchDeleteMessage(batchDeleteMessageRequest, BatchDeleteMessageCallback, nativeQueue);
                    _autoSetEvent.WaitOne();

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Batch delete message failed, exception info: " + ex.Message);
                }
            }

            #endregion

            #region Clean Generated Queue

            /* 5.2. Async delete queue again */
            try
            {
                _client.BeginDeleteQueue(deleteQueueRequest, DeleteQueueCallback, _queueName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Delete queue failed again, exception info: " + ex.Message);
            }

            _autoSetEvent.WaitOne();

            #endregion

            Console.ReadKey();
        }

        #endregion
    }
}
