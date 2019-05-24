// ***********************************************************************
// Project          : dlivetv-unofficial-api-net
// Author           : Nils Kleinert
// Created          : 05/09/2019
// 
// Last Modified On : 05/17/2019
// GitHub           : https://github.com/dlive-apis/dlivetv-api-net
// ***********************************************************************
// dlivetv-unofficial-api-net - unofficial DLive.tv API
// Copyright (C) 2019 Nils Kleinert
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
// ***********************************************************************

namespace dlivetv_unofficial_api_net.Helper
{
    public static class Querys
    {
        public static string SendStreamchatMessage = @"
                    mutation SendStreamChatMessage($input: SendStreamchatMessageInput!) {
                    sendStreamchatMessage(input: $input) {
                      err {
                        code
                        __typename
                      }
                      message {
                        type
                        ... on ChatText {
                          id
                          content
                          ...VStreamChatSenderInfoFrag
                          __typename
                        }
                        __typename
                      }
                      __typename
                    }
                  }
                  
                  fragment VStreamChatSenderInfoFrag on SenderInfo {
                    subscribing
                    role
                    roomRole
                    sender {
                      id
                      username
                      displayname
                      avatar
                      partnerStatus
                      __typename
                    }
                    __typename
                  }
            ";

        public static string DeleteChat = @"
                mutation DeleteChat($streamer: String!, $id: String!) {
                  chatDelete(streamer: $streamer, id: $id) {
                    err {
                      code
                      message
                      __typename
                    }
                    __typename
                  }
                }";

        public static string FollowUser = @"
                mutation FollowUser($streamer: String!) {
                  follow(streamer: $streamer) {
                    err {
                      code
                      message
                      __typename
                    }
                    __typename
                  }
                }
            ";

        public static string UnfollowUser = @"
                mutation UnfollowUser($streamer: String!) {
                  unfollow(streamer: $streamer) {
                    err {
                      code
                      message
                      __typename
                    }
                    __typename
                  }
                }
            ";

        public static string LivestreamPage =
            @"query LivestreamPage($displayname: String!, $add: Boolean!, $isLoggedIn: Boolean!) {
                      userByDisplayName(displayname: $displayname) {
                        id
                        ...VDliveAvatarFrag
                        ...VDliveNameFrag
                        ...VFollowFrag
                        ...VSubscriptionFrag
                        offlineImage
                        banStatus
                        deactivated
                        about
                        avatar
                        myRoomRole @include(if: $isLoggedIn)
                        isMe @include(if: $isLoggedIn)
                        isSubscribing @include(if: $isLoggedIn)
                        ...LivestreamInfoFrag
                        livestream {
                          id
                          permlink
                          watchTime(add: $add)
                          ...VVideoPlayerFrag
                          __typename
                        }
                        hostingLivestream {
                          id
                          creator {
                            ...VDliveAvatarFrag
                            displayname
                            username
                            __typename
                          }
                          ...VVideoPlayerFrag
                          __typename
                        }
                        ...LivestreamProfileFrag
                        __typename
                      }
                    }

                    fragment LivestreamInfoFrag on User {
                      id
                      livestream {
                        category {
                          title
                          imgUrl
                          id
                          backendID
                          __typename
                        }
                        title
                        watchingCount
                        totalReward
                        ...VDonationGiftFrag
                        ...VPostInfoShareFrag
                        __typename
                      }
                      ...TreasureChestFrag
                      __typename
                    }

                    fragment VDonationGiftFrag on Post {
                      permlink
                      creator {
                        username
                        __typename
                      }
                      __typename
                    }

                    fragment VPostInfoShareFrag on Post {
                      permlink
                      title
                      content
                      category {
                        id
                        backendID
                        title
                        __typename
                      }
                      __typename
                    }

                    fragment TreasureChestFrag on User {
                      id
                      username
                      treasureChest {
                        value
                        state
                        ongoingGiveaway {
                          closeAt
                          pricePool
                          claimed @include(if: $isLoggedIn)
                          __typename
                        }
                        __typename
                      }
                      ...TreasureChestPopupFrag
                      __typename
                    }

                    fragment TreasureChestPopupFrag on User {
                      id
                      username
                      isMe @include(if: $isLoggedIn)
                      isFollowing @include(if: $isLoggedIn)
                      treasureChest {
                        value
                        state
                        expireAt
                        buffs {
                          type
                          boost
                          __typename
                        }
                        ongoingGiveaway {
                          pricePool
                          closeAt
                          claimed @include(if: $isLoggedIn)
                          durationInSeconds
                          __typename
                        }
                        startGiveawayValueThreshold
                        __typename
                      }
                      __typename
                    }

                    fragment VDliveAvatarFrag on User {
                      avatar
                      __typename
                    }

                    fragment VDliveNameFrag on User {
                      displayname
                      partnerStatus
                      __typename
                    }

                    fragment LivestreamProfileFrag on User {
                      isMe @include(if: $isLoggedIn)
                      canSubscribe
                      private @include(if: $isLoggedIn) {
                        subscribers {
                          totalCount
                          __typename
                        }
                        __typename
                      }
                      videos {
                        totalCount
                        __typename
                      }
                      pastBroadcasts {
                        totalCount
                        __typename
                      }
                      followers {
                        totalCount
                        __typename
                      }
                      following {
                        totalCount
                        __typename
                      }
                      ...ProfileAboutFrag
                      __typename
                    }

                    fragment ProfileAboutFrag on User {
                      id
                      about
                      __typename
                    }

                    fragment VVideoPlayerFrag on Livestream {
                      disableAlert
                      category {
                        id
                        title
                        __typename
                      }
                      language {
                        language
                        __typename
                      }
                      __typename
                    }

                    fragment VFollowFrag on User {
                      id
                      username
                      displayname
                      isFollowing @include(if: $isLoggedIn)
                      isMe @include(if: $isLoggedIn)
                      followers {
                        totalCount
                        __typename
                      }
                      __typename
                    }

                    fragment VSubscriptionFrag on User {
                      id
                      username
                      displayname
                      isSubscribing @include(if: $isLoggedIn)
                      canSubscribe
                      isMe @include(if: $isLoggedIn)
                      subSetting {
                        benefits
                        __typename
                      }
                      __typename
                    }
                    ";
    }
}