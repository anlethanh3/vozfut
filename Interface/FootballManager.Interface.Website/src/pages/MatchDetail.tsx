import { useEffect } from "react";
import { Button, Col, Row, Table, Alert, Form, OverlayTrigger, Tooltip } from "react-bootstrap";
import moment from 'moment';
import { selectState, fetchAsync, rollingAsync, onCloseError, onShowRivals, fetchMembersAsync, onShowAdd, MatchDetailProps, addMatchDetailAsync, deleteMatchDetailAsync, fetchMatchAsync, updateMatchDetailAsync, RollingProps, onShowExchange, exchangeMembersAsync, ExchangeMemberProps, onShowUpdateRivalMember, UpdateRivalMemberProps, updateRivalMemberAsync, onSelectedId, onShowGoal, GoalProps, onShowFlip, squadAsync } from '../slices/matchDetailSlice';
import { useAppDispatch, useAppSelector } from "../app/hooks";
import { useParams } from "react-router-dom";
import Rivals from "../components/Rivals";
import AddMatchDetail from "../components/AddMatchDetail";
import { FaExchangeAlt, FaFutbol, } from "react-icons/fa";
import { GrScorecard } from "react-icons/gr";
import { MdPersonAdd, MdFlip } from "react-icons/md";
import ExchangeMember from "../components/ExchangeMember";
import UpdateRivalMember from "../components/UpdateRivalMember";
import Goals from "../components/Goals";
import FlipMember from "../components/FlipMember";

export default function MatchDetail() {
    let { id } = useParams()
    var matchId = parseInt(id ?? "")
    moment.defaultFormat = 'YYYY-MM-DD HH:mm:ss'
    const state = useAppSelector(selectState)
    const dispatch = useAppDispatch()

    const fetch = () => {
        dispatch(fetchAsync({ id: matchId })).unwrap()
            .then(value => dispatch(fetchMembersAsync(matchId)).unwrap())
            .catch(ex => { console.log(ex) })
    }

    const rolling = () => {
        dispatch(rollingAsync({ id: parseInt(id ?? "") }))
            .unwrap()
            .then(values => dispatch(onShowRivals(true)))
            .catch(error => {
            })
    }

    const onAddSubmit = (detail: MatchDetailProps) => {
        dispatch(onShowAdd(false))
        dispatch(addMatchDetailAsync(detail)).unwrap()
            .then(value => fetch())
            .catch(err => { })
    }

    const onDelete = (id: number) => {
        dispatch(deleteMatchDetailAsync(id)).unwrap()
            .then(value => fetch())
            .catch(err => { })
    }

    useEffect(() => {
        console.log('effect', state)
        dispatch(fetchMatchAsync({ id: matchId })).unwrap()
            .then(value => fetch())
            .catch(ex => { console.log(ex) })

        return function cleanup() {
            console.log('unmount', state)
        }
    }, [])

    function memberReadyCount(): number {
        var count = 0
        state.data.forEach(item => {
            if (item.isPaid === true && item.isSkip === false) {
                count += 1
            }
        })
        return count
    }

    function isInvalidRivals() {
        // if (state.match === undefined) {
        //     return true
        // }
        // var count = memberReadyCount()
        // if (count < state.match.teamCount * state.match.teamSize) {
        //     return true
        // }
        return false
    }
    function onSubmitRivals(rivals: RollingProps[]) {
        dispatch(onShowRivals(false))
    }

    const updateDetail = (data: MatchDetailProps) => {
        dispatch(updateMatchDetailAsync({ data: data })).unwrap()
            .then(value => fetch())
            .catch(ex => { console.log(ex) })
    }

    const exchangeMembers = (data: ExchangeMemberProps) => {
        dispatch(exchangeMembersAsync({ data: { ...data, matchId: matchId } })).unwrap()
            .then(value => dispatch(onShowExchange(false)))
            .catch(ex => { console.log(ex) })
    }

    const onConfirmRivalMember = (data: UpdateRivalMemberProps) => {
        dispatch(updateRivalMemberAsync({ data: { ...data, matchId: matchId } })).unwrap()
            .then(value => dispatch(onShowUpdateRivalMember(false)))
            .catch(ex => { console.log(ex) })
    }

    const onSelected = (id: number) => {
        dispatch(onSelectedId(id))
        dispatch(onShowGoal(true))
    }
    const getGoal = (id: number): GoalProps | undefined => {
        var data: GoalProps = { matchDetailId: 0, assist: 0, goal: 0 }
        var array = state.data.filter((v) => v.id === id).map(v => {
            data.assist = v.assist
            data.goal = v.goal
            data.matchDetailId = v.id
            return data
        })
        if (array.length > 0) {
            return array[0]
        }
        return undefined
    }

    const onUpdateGoal = (model: GoalProps) => {
        var array = state.data.filter((v) => v.id === state.selectedId)
        console.log(model, array)
        if (array.length > 0) {
            updateDetail({ ...array[0], assist: model.assist, goal: model.goal })
        }
        dispatch(onShowGoal(false))
    }

    const onUpdateSquad = async (model: RollingProps[], matchId: number) => {
        dispatch(onShowFlip(false))
        dispatch(squadAsync({ data: model, matchId: matchId })).unwrap()
            .then(value => { rolling() })
            .catch(ex => { })
    }

    return (
        <>
            {
                state.match &&
                <>
                    <h1>{state.match.name}</h1>
                    <h2>{state.match.description}</h2>
                </>
            }
            {
                state.isLoading &&
                <Alert color='primary'>Loading...</Alert>
            }
            {
                state.isShowAdd &&
                <AddMatchDetail show={state.isShowAdd} matchId={parseInt(id ?? "")}
                    members={state.members} onSubmit={(detail) => { onAddSubmit(detail) }} onClose={() => dispatch(onShowAdd(false))} />
            }
            {
                state.isShowRivals && state.rolling && state.match &&
                <Rivals onSubmit={(rivals) => onSubmitRivals(rivals)} show={state.isShowRivals} rivals={state.rolling} match={state.match} onClose={() => dispatch(onShowRivals(false))} />
            }
            {
                state.isShowExchange && state.members &&
                <ExchangeMember onSubmit={(data) => exchangeMembers(data)} show={state.isShowExchange} members={state.members} onClose={() => dispatch(onShowExchange(false))} />
            }
            {
                state.error &&
                <Alert show={state.error !== undefined} variant="danger" onClose={() => dispatch(onCloseError())} dismissible>
                    {state.error}
                </Alert>
            }
            {
                state.isShowUpdateRivalMember && state.match && state.members &&
                <UpdateRivalMember match={state.match} onSubmit={(data) => { onConfirmRivalMember(data) }} show={state.isShowUpdateRivalMember} members={state.members} onClose={() => dispatch(onShowUpdateRivalMember(false))} />
            }
            {
                state.isShowGoal &&
                <Goals initial={getGoal(state.selectedId)} onSubmit={(value) => onUpdateGoal(value)} show={state.isShowGoal} onClose={() => dispatch(onShowGoal(false))} />
            }
            {
                state.isShowFlip && state.match &&
                <FlipMember match={state.match} initial={state.members} onSubmit={(data) => { onUpdateSquad(data, state.match?.id ?? 0) }} show={state.isShowFlip} onClose={() => dispatch(onShowFlip(false))} />
            }
            <Row className="my-2">
                <Col className="d-flex justify-content-end">
                    <OverlayTrigger overlay={<Tooltip>Add Team Member</Tooltip>}>
                        <Button className="me-2" disabled={isInvalidRivals()} variant="danger" onClick={() => { dispatch(onShowUpdateRivalMember(true)) }}><MdPersonAdd /></Button>
                    </OverlayTrigger>
                    <OverlayTrigger overlay={<Tooltip>In Out Members</Tooltip>}>
                        <Button className="me-2" disabled={isInvalidRivals()} variant="secondary" onClick={() => { dispatch(onShowExchange(true)) }}><FaExchangeAlt /></Button>
                    </OverlayTrigger>
                    <OverlayTrigger overlay={<Tooltip>Team Division Rivals</Tooltip>}>
                        <Button className="me-2" disabled={isInvalidRivals()} variant="success" onClick={() => { rolling() }}><FaFutbol /></Button>
                    </OverlayTrigger>
                    <OverlayTrigger overlay={<Tooltip>Flip Team Member</Tooltip>}>
                        <Button variant="info" onClick={() => { dispatch(onShowFlip(true)) }}><MdFlip /></Button>
                    </OverlayTrigger>
                </Col>
            </Row>
            {
                <h3>Member paid count: {memberReadyCount()}</h3>
            }
            <Table striped bordered hover>
                <thead>
                    <tr>
                        <th>No</th>
                        <th>Member</th>
                        <th>Is paid</th>
                        <th>Is Skip</th>
                        <th>Goal</th>
                        <th>Assist</th>
                        <th>Modified Date</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {
                        state.data && state.data.map((value, index) =>
                            <tr key={`key-${index}`}>
                                <td>{index + 1}</td>
                                <td>{value.memberName}</td>
                                <td><Form.Check onChange={(e) => { updateDetail({ ...state.data[index], isPaid: e.target.checked }) }} checked={value.isPaid} type="switch" /></td>
                                <td><Form.Check onChange={(e) => { updateDetail({ ...state.data[index], isSkip: e.target.checked }) }} checked={value.isSkip} type="switch" /></td>
                                <td>{value.goal}</td>
                                <td>{value.assist}</td>
                                <td>{value.modifiedDate && moment(value.modifiedDate).format()}</td>
                                <td>
                                    <OverlayTrigger overlay={<Tooltip>Goals</Tooltip>}>
                                        <Button className="me-2" disabled={isInvalidRivals()} variant="secondary" onClick={() => { onSelected(value.id) }}><GrScorecard /></Button>
                                    </OverlayTrigger>
                                </td>
                            </tr>
                        )
                    }
                </tbody>
            </Table>
        </>
    )
}